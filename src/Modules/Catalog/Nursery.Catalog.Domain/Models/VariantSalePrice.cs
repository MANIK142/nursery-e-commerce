namespace Nursery.Catalog.Domain.Models;

public class VariantSalePrice : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid PlantVariantId { get; private set; }
    public Money SalePrice { get; private set; } = default!;
    public DateTime StartsAtUtc { get; private set; }
    public DateTime EndsAtUtc { get; private set; }

    public bool IsActiveAt(DateTime asOfUtc) => asOfUtc >= StartsAtUtc && asOfUtc <= EndsAtUtc;

    // Two ranges overlap unless one ends before the other starts.
    public bool OverlapsWith(DateTime otherStartsAtUtc, DateTime otherEndsAtUtc)
        => StartsAtUtc < otherEndsAtUtc && otherStartsAtUtc < EndsAtUtc;

    private VariantSalePrice() { } // EF Core

    private VariantSalePrice(Guid id, Guid plantVariantId, Money salePrice, DateTime startsAtUtc, DateTime endsAtUtc)
    {
        Id = id;
        PlantVariantId = plantVariantId;
        SalePrice = salePrice;
        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;
    }

    internal static VariantSalePrice Create(
        Guid plantVariantId, Money salePrice, DateTime startsAtUtc, DateTime endsAtUtc, string createdBy)
    {
        if (salePrice is null)
            throw new ArgumentNullException(nameof(salePrice), "Sale price cannot be null.");
        if (salePrice.Amount < 0)
            throw new ArgumentOutOfRangeException(nameof(salePrice), "Sale price cannot be negative.");
        if (startsAtUtc < DateTime.UtcNow)
            throw new ArgumentOutOfRangeException(nameof(startsAtUtc), "Sale start time cannot be in the past.");
        if (startsAtUtc >= endsAtUtc)
            throw new ArgumentOutOfRangeException(nameof(startsAtUtc), "Sale start time must be before its end time.");

        var variantSalePrice = new VariantSalePrice(Guid.NewGuid(), plantVariantId, salePrice, startsAtUtc, endsAtUtc);
        variantSalePrice.SetCreated(createdBy);
        return variantSalePrice;
    }

  
    public void EndSale(string modifiedBy)
    {
        EndsAtUtc = DateTime.UtcNow;
        SetModified(modifiedBy);
    }
}