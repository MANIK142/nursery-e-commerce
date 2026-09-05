using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Nursery.Catalog.Domain.Models;

public class Plant : BaseDomainModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string ImageUrl { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    private readonly List<PlantCategory> _categories = new();
    public IReadOnlyCollection<Guid> CategoryIds => _categories.Select(pc => pc.CategoryId).ToList();

    private readonly List<PlantVariant> _variants = new();
    public IReadOnlyCollection<PlantVariant> Variants => _variants.AsReadOnly();

    private Plant() { }

    private Plant(Guid id, string name, string description,  string imageUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public static Plant Create( string name, string description,  string imageUrl, string createdBy, List<PlantVariantSpec> plantVariantSpecs)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Plant name is required.", nameof(name));
 
        var plant = new Plant(Guid.NewGuid(), name, description, imageUrl);
        plant.SetCreated(createdBy);
        foreach (var plantVariantSpec in plantVariantSpecs)
        {
            var plantVariantSpecPlantId = new PlantVariantSpecWithPlantId(plant.Id, plantVariantSpec.Sku, plantVariantSpec.VariantName,
                                                    plantVariantSpec.RetailPrice, plantVariantSpec.WholesalePrice);

            //var plantVariant = PlantVariant.Create(plant.Id, plantVariantSpec.Sku, plantVariantSpec.VariantName, 
            //                                        plantVariantSpec.RetailPrice, plantVariantSpec.WholesalePrice, createdBy);
 
            plant.AddVariant(plantVariantSpecPlantId, createdBy);
        }  
        return plant;
    }

    public PlantVariant AddVariant(PlantVariantSpecWithPlantId plantVariantSpec, string modifiedBy)
    {
        if (plantVariantSpec is null)
            throw new ArgumentNullException(nameof(plantVariantSpec), "Variant cannot be null.");
        if (_variants.Any(v => v.Sku == plantVariantSpec.Sku))
            throw new InvalidOperationException($"A variant with SKU '{plantVariantSpec.Sku}' already exists for this plant.");

        var variant = PlantVariant.Create(plantVariantSpec.PlantId, plantVariantSpec.Sku, plantVariantSpec.VariantName,
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

    public void SetImage(string? imageUrl, string modifiedBy)
    {
        ImageUrl = imageUrl;
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

public record PlantVariantSpec(string Sku, string VariantName, Money RetailPrice, Money WholesalePrice);
public record PlantVariantSpecWithPlantId(Guid PlantId, string Sku, string VariantName, Money RetailPrice, Money WholesalePrice);

public record PlantVariantSpecWithId(Guid Id,string Sku, string VariantName, Money RetailPrice, Money WholesalePrice);