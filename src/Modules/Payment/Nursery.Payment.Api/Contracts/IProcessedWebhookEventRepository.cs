namespace Nursery.Payment.Api.Contracts;

public interface IProcessedWebhookEventRepository
{
    Task<bool> ExistsAsync(string eventId, CancellationToken ct);
    Task MarkProcessedAsync(string eventId, CancellationToken ct);
}
