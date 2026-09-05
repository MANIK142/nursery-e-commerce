using System.ComponentModel.DataAnnotations.Schema;

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


    private Plant() { }

    private Plant(Guid id, string name, string description,  string imageUrl)
    {
        Id = id;
        Name = name;
        Description = description;
        ImageUrl = imageUrl;
    }

    public static Plant Create( string name, string description,  string imageUrl, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Plant name is required.", nameof(name));
 
        var plant = new Plant(Guid.NewGuid(), name, description, imageUrl);
        plant.SetCreated(createdBy);
        return plant;
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
