namespace Nursery.Catalog.Domain.Models;
public class PlantImage : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid? PlantId { get; private set; }
    public Guid? PlantVariantId { get; private set; }
    public string AltText { get; private set; } = default!;
    public string StorageKey { get; private set; } = default!;
    public bool IsPrimaryImage { get; private set; }

    private PlantImage() { }
    internal static PlantImage CreateForPlant(Guid plantId, string altText, string storageKey, bool isPrimary)
    {
        if (plantId == Guid.Empty) throw new ArgumentException("PlantId cannot be empty.", nameof(plantId));
        if (string.IsNullOrWhiteSpace(storageKey)) throw new ArgumentNullException(nameof(storageKey));

        return new PlantImage
        {
            Id = Guid.NewGuid(),
            PlantId = plantId,
            AltText = altText,
            StorageKey = storageKey,
            IsPrimaryImage = isPrimary
        };
    }
    internal static PlantImage CreateForVariant(Guid variantId, string altText, string storageKey, bool isPrimary)
    {
        if (variantId == Guid.Empty) throw new ArgumentException("VariantId cannot be empty.", nameof(variantId));
        if (string.IsNullOrWhiteSpace(storageKey)) throw new ArgumentNullException(nameof(storageKey));

        return new PlantImage
        {
            Id = Guid.NewGuid(),
            PlantVariantId = variantId,
            AltText = altText,
            StorageKey = storageKey,
            IsPrimaryImage = isPrimary
        };
    }
    public void SetPrimaryImage(string modifiedBy)
    {
        IsPrimaryImage = true;
        SetModified(modifiedBy);
    }
    public void RemoveAsPrimaryImage(string modifiedBy)
    {
        IsPrimaryImage = false;
        SetModified(modifiedBy);
    }
    public void ChangeStorageKey(string key, string modifiedBy)
    {
        StorageKey = key;
        SetModified(modifiedBy);
    }
    public void ChangeAltText(string altText, string modifiedBy)
    {
        AltText = altText;
        SetModified(modifiedBy);
    }
}
