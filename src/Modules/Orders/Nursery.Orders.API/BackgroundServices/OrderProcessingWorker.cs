using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Domain.Orders;
using Nursery.Orders.Infrastructure.Persistance.Context;
using Nursery.Orders.Infrastructure.Persistance.Repository;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Nursery.Orders.API.BackgroundServices;

public class OrderProcessingWorker : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceScopeFactory _scopeFactory;
    public OrderProcessingWorker(IConnection connection, IServiceScopeFactory scopeFactory)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        // 1. Declare Dead Letter Exchange and Queue
        await channel.ExchangeDeclareAsync("orders.dlx", ExchangeType.Direct, durable: true, cancellationToken: stoppingToken);
        await channel.QueueDeclareAsync("orders.poison.dlq", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
        await channel.QueueBindAsync("orders.poison.dlq", "orders.dlx", "orders.poison", cancellationToken: stoppingToken);

        // 2. Declare Main Quorum Queue with DLX arguments
        var queueArgs = new Dictionary<string, object?>
        {
            ["x-queue-type"] = "quorum",
            ["x-dead-letter-exchange"] = "orders.dlx",
            ["x-dead-letter-routing-key"] = "orders.poison",
            ["x-delivery-limit"] = 3 // Quorum queue maximum delivery attempts
        };

        await channel.ExchangeDeclareAsync(
                                    exchange: "paymentstatus.exchange",
                                    type: ExchangeType.Direct,
                                    durable: true,
                                    autoDelete: false,
                                    cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(
                        queue: "orders.fulfillment",
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: queueArgs,
                        cancellationToken: stoppingToken);

        await channel.QueueBindAsync("orders.fulfillment", "paymentstatus.exchange", "payment.success", cancellationToken: stoppingToken);
        await channel.QueueBindAsync("orders.fulfillment", "paymentstatus.exchange", "payment.failed", cancellationToken: stoppingToken);

        // 3. Set Fair Dispatching (Prefetch QoS)
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken: stoppingToken);

        // 4. Start Consumer with explicit ACKs (autoAck = false)
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var deliveryTag = ea.DeliveryTag;
            var messageIdStr = ea.BasicProperties.MessageId;
            var routingKey = ea.RoutingKey;

            if (!Guid.TryParse(messageIdStr, out var messageId))
            {
                // Reject invalid or malformed messages immediately to the DLQ
                await channel.BasicRejectAsync(deliveryTag, requeue: false);
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            var _orderRepository = scope.ServiceProvider.GetRequiredService<OrderRepository>();
            var _cartRepository = scope.ServiceProvider.GetRequiredService<CartReposiotry>();

            try
            {
                // 5. Check Idempotency and process logic
                var success = await HandleOrderFulfillmentIdempotently(messageId, routingKey, ea.Body.ToArray(), dbContext, _orderRepository, _cartRepository);

                if (success)
                {
                    await channel.BasicAckAsync(deliveryTag, multiple: false);
                }
                else
                {
                    // Business rule rejected: Route to DLQ
                    await channel.BasicRejectAsync(deliveryTag, requeue: false);
                }
            }
            catch (TransientException)
            {
                // Transient network/DB issue: Nack and requeue
                await channel.BasicNackAsync(deliveryTag, multiple: false, requeue: true);
            }
            catch (Exception)
            {
                // Unrecoverable poison payload: Reject to DLX
                await channel.BasicRejectAsync(deliveryTag, requeue: false);
            }
        };

        await channel.BasicConsumeAsync(queue: "orders.fulfillment", autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
    }

    private async Task<bool> HandleOrderFulfillmentIdempotently(Guid messageId, string routingKey, byte[] body, OrdersDbContext dbContext, OrderRepository _orderRepository, CartReposiotry _cartRepository)
    {
        // Check if message was already handled (Idempotency)
        if (await dbContext.ProcessedMessages.AnyAsync(m => m.MessageId == messageId))
        {
            return true; // Already processed; acknowledge and ignore
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync();

        // Parse content
        var payload = JsonSerializer.Deserialize<JsonElement>(Encoding.UTF8.GetString(body));
        var orderId = payload.GetProperty("OrderId").GetGuid();
       
        var ct = new CancellationToken();
        using var scope = _scopeFactory.CreateScope();
        var order = await _orderRepository.GetByIdAsync(orderId, ct);
        if (order is null)
        {
            //logger.LogWarning(
            //    "PaymentSucceeded received for unknown Order {OrderId} (Payment {PaymentId}).",
            //    notification.OrderId, notification.PaymentId);
            return false;
        }

        switch (routingKey)
        {
            case "payment.success":
                order.PlaceOrder();
                order.MarkAsPaid();
                await _orderRepository.SaveChangesAsync(ct);

                var cart = await _cartRepository.GetActiveCartByCustomerIdAsync(order.CustomerId, ct);
                foreach (var cartItem in cart.Items.ToList())
                {
                    cart.RemoveItemFromCart(cartItem.PlantvariantId);
                }
                await _cartRepository.SaveChangesAsync(ct);
             break;
            case "payment.failed":
                order.CancelOrder();
                order.MarkPaymentFailed();
                await _orderRepository.SaveChangesAsync(ct);
                break;
            default:
                return false;
        }
        


        // Mark message as processed inside the same transaction
        dbContext.ProcessedMessages.Add(new ProcessedMessage
                                        {
                                            MessageId = messageId,
                                            ProcessedAtUtc = DateTime.UtcNow
                                        });

        await dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
    }
}

public class TransientException : Exception { }
