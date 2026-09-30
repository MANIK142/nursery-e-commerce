namespace Nursery.Payment.Api.Services;

using Microsoft.EntityFrameworkCore;
using Nursery.Payment.Api.Persistance;
using RabbitMQ.Client;
using System.Text;

public class OutboxPublisherService :BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConnection _connection;
    public OutboxPublisherService(IServiceScopeFactory scopeFactory, IConnection connection)
    {
        _scopeFactory = scopeFactory;
        _connection = connection;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channelOptions = new CreateChannelOptions(
            publisherConfirmationsEnabled: true,
            publisherConfirmationTrackingEnabled: true
        );

        await using var channel = await _connection.CreateChannelAsync(channelOptions, stoppingToken);

        // Declare exchange
        await channel.ExchangeDeclareAsync(
            exchange: "paymentstatus.exchange",
            type: ExchangeType.Direct,
            durable: true,
            cancellationToken: stoppingToken);



        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<PaymentDbContext>();

            // Fetch a batch of pending events
            var batch = await dbContext.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(20)
                .ToListAsync(stoppingToken);

            if (batch.Count == 0)
            {
                await Task.Delay(1000, stoppingToken);
                continue;
            }

            foreach (var message in batch)
            {
                try
                {
                    var props = new BasicProperties
                    {
                        MessageId = message.Id.ToString(),
                        DeliveryMode = DeliveryModes.Persistent, 
                        ContentType = "application/json"
                    };

                    byte[] body = Encoding.UTF8.GetBytes(message.Content);

                    // 2. Publish and wait for broker confirmation
                    await channel.BasicPublishAsync(
                        exchange: "paymentstatus.exchange",
                        routingKey: message.Type,
                        mandatory: true,
                        basicProperties: props,
                        body: body,
                        cancellationToken: stoppingToken);

                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    message.Error = ex.Message;
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
