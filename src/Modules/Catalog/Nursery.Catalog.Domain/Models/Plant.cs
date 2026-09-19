using Nursery.Catalog.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace Nursery.Catalog.Domain.Models;

public class Plant : BaseDomainModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    public CareInstruction? CareInstruction { get; private set; } = default!;

    private readonly List<PlantImage> _images = new();       // generic/hero images
    public IReadOnlyCollection<PlantImage> Images => _images.AsReadOnly();

    private readonly List<PlantCategory> _categories = new();
    public IReadOnlyCollection<PlantCategory> Categories => _categories.AsReadOnly();
    public IReadOnlyCollection<Guid> CategoryIds => _categories.Select(pc => pc.CategoryId).ToList();

    private readonly List<PlantVariant> _variants = new();
    public IReadOnlyCollection<PlantVariant> Variants => _variants.AsReadOnly();

    private Plant() { }

    private Plant(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static Plant Create( string name, string description,string createdBy, List<PlantVariantSpec> plantVariantSpecs,List<ImageSpec> imageSpecs)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Plant name is required.", nameof(name));
 
        var plant = new Plant(Guid.NewGuid(), name, description);

        plant.Raise(new PlantCreatedEvent(plant.Id, plant.Name, plant.Description));

        plant.SetCreated(createdBy);
        foreach (var plantVariantSpec in plantVariantSpecs)
        {
            var plantVariantSpecPlantId = new PlantVariantSpecWithPlantId(plant.Id, plantVariantSpec.Sku, plantVariantSpec.VariantName,plantVariantSpec.ImageSpecs,
                                                    plantVariantSpec.RetailPrice, plantVariantSpec.WholesalePrice);
            plant.AddVariant(plantVariantSpecPlantId, createdBy);
        }  

        foreach(var imagesepc in imageSpecs)
        {
            plant.AddPlantImage(plant.Id, imagesepc, createdBy);
        }
        return plant;
    }

    public PlantImage AddPlantImage(Guid PlantId, ImageSpec ImageSpec, string modifiedBy)
    {
        if (ImageSpec is null)
            throw new ArgumentNullException(nameof(ImageSpec), "Variant cannot be null.");
        if (_images.Any(v => v.StorageKey == ImageSpec.StorageKey))
            throw new InvalidOperationException($"A Image with storagekey '{ImageSpec.StorageKey}' already exists for this plant.");

        var plantImage = PlantImage.CreateForPlant(PlantId, ImageSpec.AltText, ImageSpec.StorageKey, ImageSpec.IsPrimaryImage);
        _images.Add(plantImage);
        SetModified(modifiedBy);
        return plantImage;
    }

    public void UpdatePlantImage(PlantVariantSpecWithId newVariant, string modifiedBy)
    {
        //var variant = _images.FirstOrDefault(v => v.Id == newVariant.Id);
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


    public PlantVariant AddVariant(PlantVariantSpecWithPlantId plantVariantSpec, string modifiedBy)
    {
        if (plantVariantSpec is null)
            throw new ArgumentNullException(nameof(plantVariantSpec), "Variant cannot be null.");
        if (_variants.Any(v => v.Sku == plantVariantSpec.Sku))
            throw new InvalidOperationException($"A variant with SKU '{plantVariantSpec.Sku}' already exists for this plant.");

        var variant = PlantVariant.Create(plantVariantSpec.PlantId, plantVariantSpec.Sku, plantVariantSpec.VariantName,plantVariantSpec.ImageSpecs,
                                                    plantVariantSpec.RetailPrice, plantVariantSpec.WholesalePrice, modifiedBy);


        _variants.Add(variant);
        SetModified(modifiedBy);
        return variant;

    }


    public void UpdateVariant(PlantVariantSpecWithId newVariant, string modifiedBy)
    {
        var variant = _variants.FirstOrDefault(v => v.Id == newVariant.Id);
        if (variant is null)
            throw new InvalidOperationException($"No variant with ID '{newVariant.Id}' exists for this plant.");

        variant.UpdateVariantName(newVariant.VariantName, modifiedBy);
        variant.UdpateSku(newVariant.Sku, modifiedBy);
        variant.UdpateRetailPrice(newVariant.RetailPrice, modifiedBy);
        variant.UdpateWholesalePrice(newVariant.WholesalePrice, modifiedBy);
       
    }
    public void RemoveVariant(Guid variantId, string modifiedBy)
    {
        var variant = _variants.FirstOrDefault(v => v.Id == variantId);
        if (variant is null)
            throw new InvalidOperationException($"No variant with ID '{variantId}' exists for this plant.");
        _variants.Remove(variant);
        SetModified(modifiedBy);
    }

    public void AddCategory(Guid categoryId, string modifiedBy)
    {
        if (categoryId == Guid.Empty)
            throw new ArgumentException("Invalid category id.", nameof(categoryId));
        if (_categories.Any(pc => pc.CategoryId == categoryId))
            return; // already categorized this way — no-op, not an error

        _categories.Add(PlantCategory.Create(Id, categoryId));
        SetModified(modifiedBy);
    }

    public void RemoveCategory(Guid categoryId, string modifiedBy)
    {
        if (_categories.Count == 1 && _categories[0].CategoryId == categoryId)
            throw new InvalidOperationException("A plant must belong to at least one category.");

        _categories.RemoveAll(pc => pc.CategoryId == categoryId);
        SetModified(modifiedBy);
    }


    public void Deactivate(string modifiedBy)
    {
        IsActive = false;
        SetModified(modifiedBy);
    }

    public void SetDescription(string description, string modifiedBy)
    {
        Description = description;
        SetModified(modifiedBy);
    }

    public void SetName(string name, string modifiedBy)
    {
        Name = name;
        SetModified(modifiedBy);
    }
}

public record PlantVariantSpec(string Sku, string VariantName, List<ImageSpec> ImageSpecs, Money RetailPrice, Money WholesalePrice,List<SalePriceSpec> SalePrices);
public record PlantVariantSpecWithPlantId(Guid PlantId, string Sku, string VariantName, List<ImageSpec> ImageSpecs, Money RetailPrice, Money WholesalePrice);
public record PlantVariantSpecWithId(Guid Id,string Sku, string VariantName, Money RetailPrice, Money WholesalePrice);
public record ImageSpec(string StorageKey, bool IsPrimaryImage, string AltText);

public record SalePriceSpec(Money SalePrice, DateTime StartsAtUtc, DateTime EndsAtUtc);