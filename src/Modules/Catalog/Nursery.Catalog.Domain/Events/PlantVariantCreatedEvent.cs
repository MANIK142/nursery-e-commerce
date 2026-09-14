

using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Domain.Events;

public sealed record PlantVariantCreatedEvent(Guid Id,Guid plantId, string Sku, string VariantName, List<ImageSpec> ImageSpecs, Money RetailPrice, Money WholesalePrice, string CreatedBy) : IDomainEvent
{
    public DateTime OccurredOnUtc { get; } = DateTime.UtcNow;
}
