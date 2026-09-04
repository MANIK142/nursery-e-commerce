
namespace Nursery.Catalog.Domain.Models;
public class Category :BaseDomainModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }

    private Category() { } 

    private Category(Guid id, string name, string? description)
    {
        Id = id;
        Name = name;
        Description = description;
    }


    public static Category Create(string name, string? description, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.", nameof(name));

        var category = new Category(Guid.NewGuid(), name, description);
        category.SetCreated(createdBy);
        return category;
    }

    public void Rename(string name, string modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.", nameof(name));

        Name = name;
        SetModified(modifiedBy);
    }

    public void UpdateDescription(string? description, string modifiedBy)
    {
        Description = description;
        SetModified(modifiedBy);
    }
}
