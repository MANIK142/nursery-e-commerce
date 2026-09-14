using Nursery.Catalog.Domain.Events;

namespace Nursery.Catalog.Domain.Models;

public class PlantVariant : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid PlantId { get; private set; }
    public string Sku { get; private set; } = default!;
    public string VariantName { get; private set; } = default!;
    public Money RetailPrice { get; private set; } = default!;
    public Money WholesalePrice { get; private set; } = default!;
    public bool IsActive { get; private set; }
    private readonly List<VariantSalePrice> _salePrices = new();
    public IReadOnlyCollection<VariantSalePrice> SalePrices => _salePrices.AsReadOnly();

    private readonly List<PlantImage> _images = new();
    public IReadOnlyCollection<PlantImage> Images => _images.AsReadOnly();

    private PlantVariant() { } // EF Core
    private PlantVariant(Guid id, Guid plantId, string sku, string variantName, Money retailPrice, Money wholesalePrice)
    {
        Id = id;
        PlantId = plantId;
        Sku = sku;
        VariantName = variantName;
        RetailPrice = retailPrice;
        WholesalePrice = wholesalePrice;
        IsActive = true;
    }
    internal static PlantVariant Create(
        Guid plantId, string sku, string variantName, List<ImageSpec> ImageSpecs, Money retailPrice, Money wholesalePrice, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU code is required.", nameof(sku));
        if (string.IsNullOrWhiteSpace(variantName))
            throw new ArgumentException("Variant name is required.", nameof(variantName));
        if (retailPrice.Amount < 0)
            throw new ArgumentException("Retail price cannot be negative.", nameof(retailPrice));
        if (wholesalePrice.Amount < 0)
            throw new ArgumentException("Wholesale price cannot be negative.", nameof(wholesalePrice));

        var plantVariant = new PlantVariant(Guid.NewGuid(), plantId, sku, variantName, retailPrice, wholesalePrice);

        plantVariant.Raise(new PlantVariantCreatedEvent(plantVariant.Id, plantId, sku, variantName, ImageSpecs, retailPrice, wholesalePrice,createdBy));

        plantVariant.SetCreated(createdBy);
        foreach(var imageSpec in ImageSpecs)
        {
            plantVariant.AddPlantImage(plantVariant.Id, imageSpec, createdBy);
        }


        
        return plantVariant;
    }
    public void StartSale(Money salePrice, DateTime startsAtUtc, DateTime endsAtUtc, string modifiedBy)
    {
        var hasOverlap = _salePrices.Any(existing => existing.OverlapsWith(startsAtUtc, endsAtUtc));
        if (hasOverlap)
            throw new InvalidOperationException(
                $"Variant '{Sku}' already has a sale that overlaps {startsAtUtc:u}\u2013{endsAtUtc:u}.");

        var variantSalePrice = VariantSalePrice.Create(Id, salePrice, startsAtUtc, endsAtUtc, modifiedBy);
        _salePrices.Add(variantSalePrice);
        SetModified(modifiedBy);
    }
    public void EndSaleEarly(string modifiedBy)
    {
        var activeSale = _salePrices.FirstOrDefault(s => s.IsActiveAt(DateTime.UtcNow));
        if (activeSale is null)
            return; // nothing active to end — no-op, not an error

        activeSale.EndSale(modifiedBy);
        SetModified(modifiedBy);
    }

    public void UpdateVariantName(string variantName,string ModifiedBy)
    {
        VariantName = variantName;
        SetModified(ModifiedBy);
    }

    public void UdpateRetailPrice(Money retailPrice, string ModifiedBy)
    {
        RetailPrice = retailPrice;
        SetModified(ModifiedBy);
    }

    public void UdpateWholesalePrice(Money wholesalePrice, string ModifiedBy)
    {
        WholesalePrice = wholesalePrice;
        SetModified(ModifiedBy);
    }

    public void UdpateSku(string sku, string ModifiedBy)
    {
        Sku = sku;
        SetModified(ModifiedBy);
    }

    public Money GetPrice(CustomerTier tier, DateTime asOfUtc)
    {
        if (tier == CustomerTier.Retail)
        {
            var activeSale = _salePrices.FirstOrDefault(s => s.IsActiveAt(asOfUtc));
            return activeSale?.SalePrice ?? RetailPrice;
        }

        return WholesalePrice;
    }
    public void Deactivate(string modifiedBy)
    {
        IsActive = false;
        SetModified(modifiedBy);
    }


    public PlantImage AddPlantImage(Guid PlantId, ImageSpec ImageSpec, string modifiedBy)
    {
        if (ImageSpec is null)
            throw new ArgumentNullException(nameof(ImageSpec), "Variant cannot be null.");
        if (_images.Any(v => v.StorageKey == ImageSpec.StorageKey))
            throw new InvalidOperationException($"A Image with storagekey '{ImageSpec.StorageKey}' already exists for this plant.");

        var plantImage = PlantImage.CreateForVariant(PlantId,ImageSpec.AltText,ImageSpec.StorageKey,ImageSpec.IsPrimaryImage);
        _images.Add(plantImage);
        SetModified(modifiedBy);
        return plantImage;
    }

    public void UpdatePlantImage(PlantVariantSpecWithId newVariant, string modifiedBy)
    {
        //var variant = _Images.FirstOrDefault(v => v.Id == newVariant.Id);
        //if (variant is null)
        //    throw new InvalidOperationException($"No variant with ID '{newVariant.Id}' exists for this plant.");

        //variant.UpdateVariantName(newVariant.VariantName, modifiedBy);
        //variant.UdpateSku(newVariant.Sku, modifiedBy);
        //variant.UdpateRetailPrice(newVariant.RetailPrice, modifiedBy);
        //variant.UdpateWholesalePrice(newVariant.WholesalePrice, modifiedBy);

    }

    public void RemovePlantImage(Guid ImageId, string modifiedBy)
    {
        var image = _images.FirstOrDefault(v => v.Id == ImageId);
        if (image is null)
            throw new InvalidOperationException($"No Image with ID '{ImageId}' exists for this plant.");
        _images.Remove(image);
        SetModified(modifiedBy);
    }
}

public enum CustomerTier { Retail, Wholesale }