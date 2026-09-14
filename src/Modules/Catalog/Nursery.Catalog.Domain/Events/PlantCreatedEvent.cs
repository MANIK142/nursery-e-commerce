
using BuildingBlocks.Common;

namespace Nursery.Catalog.Domain.Events;

public sealed record PlantCreatedEvent(Guid PlantId, string Name, string Description) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;

}
