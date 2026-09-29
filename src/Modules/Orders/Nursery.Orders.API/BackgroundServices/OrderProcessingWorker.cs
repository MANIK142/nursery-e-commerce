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
    private readonly IOrderRepository _orderRepository;
    public ICartRepository _cartRepository { get; }

    public OrderProcessingWorker(IConnection connection, IServiceScopeFactory scopeFactory,ICartRepository cartRepository,IOrderRepository orderRepository)
    {
        _connection = connection;
        _scopeFactory = scopeFactory;
        _cartRepository = cartRepository;
        _orderRepository = orderRepository;
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

        await channel.QueueDeclareAsync(
            queue: "orders.fulfillment",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArgs,
            cancellationToken: stoppingToken);

        await channel.QueueBindAsync("orders.fulfillment", "orders.exchange", "order.created", cancellationToken: stoppingToken);

        // 3. Set Fair Dispatching (Prefetch QoS)
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 10, global: false, cancellationToken: stoppingToken);

        // 4. Start Consumer with explicit ACKs (autoAck = false)
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var deliveryTag = ea.DeliveryTag;
            var messageIdStr = ea.BasicProperties.MessageId;

            if (!Guid.TryParse(messageIdStr, out var messageId))
            {
                // Reject invalid or malformed messages immediately to the DLQ
                await channel.BasicRejectAsync(deliveryTag, requeue: false);
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

            try
            {
                // 5. Check Idempotency and process logic
                var success = await HandleOrderFulfillmentIdempotently(messageId, ea.Body.ToArray(), dbContext);

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

    private async Task<bool> HandleOrderFulfillmentIdempotently(Guid messageId, byte[] body, OrdersDbContext dbContext)
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

        var order = await _orderRepository.GetByIdAsync(orderId, ct);
        if (order is null)
        {
            //logger.LogWarning(
            //    "PaymentSucceeded received for unknown Order {OrderId} (Payment {PaymentId}).",
            //    notification.OrderId, notification.PaymentId);
            return false;
        }
        order.PlaceOrder();
        order.MarkAsPaid();
        await _orderRepository.SaveChangesAsync(ct);

        var cart = await _cartRepository.GetActiveCartByCustomerIdAsync(order.CustomerId, ct);
        foreach (var cartItem in cart.Items.ToList())
        {
            cart.RemoveItemFromCart(cartItem.PlantvariantId);
        }
        await _cartRepository.SaveChangesAsync(ct);


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
