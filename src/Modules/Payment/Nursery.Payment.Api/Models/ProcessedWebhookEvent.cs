using BuildingBlocks.Common;
using Nursery.ShareKernel;

namespace Nursery.Payment.Api.Models;

public class ProcessedWebhookEvent :BaseDomainModel
{
    public Guid Id { get; private set; }
    public string EventId { get; private set; } = null!;
    public DateTime ProcessedAt { get; private set; }

    private ProcessedWebhookEvent() { } // EF Core

    public ProcessedWebhookEvent(string eventId)
    {
        Id = Guid.NewGuid();
        EventId = Guard.AgainstNullOrWhiteSpace(eventId, nameof(eventId));
        ProcessedAt = DateTime.UtcNow;
    }
}
