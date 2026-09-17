using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Nursery.Web.Host.Models.ViewModels.Plants;

public class CreatePlantViewModel
{

    [Required(ErrorMessage = "Plant name is required.")]
    [StringLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }
    public string? CreatedBy { get; set; } = string.Empty;

    [Required(ErrorMessage = "Select at least one category.")]
    [MinLength(1, ErrorMessage = "At least one category must be selected.")]
    public List<string> SelectedCategories { get; set; } = new();

    // Available categories to populate checkbox lists or multiselects
    public List<SelectListItem> AvailableCategories { get; set; } = new();

    // Nested dynamic collections
    public List<PlantVariantInputModel> PlantVariantSpecs { get; set; } = new();
    public List<PlantImageInputModel> Images { get; set; } = new();
}

public class PlantVariantInputModel
{
    [Required(ErrorMessage = "SKU is required.")]
    public string Sku { get; set; } = string.Empty;

    [Required(ErrorMessage = "Variant name is required.")]
    public string VariantName { get; set; } = string.Empty;

    public MoneyInputModel RetailPrice { get; set; } = new();
    public MoneyInputModel WholesalePrice { get; set; } = new();

    public List<PlantImageInputModel> ImageSpecs { get; set; } = new();
}

public class PlantImageInputModel
{
    [Required(ErrorMessage = "Storage key is required.")]
    public string StorageKey { get; set; } = string.Empty;

    public bool IsPrimaryImage { get; set; }

    public string? AltText { get; set; }
}

public class MoneyInputModel
{
    [Range(0.01, 100000.00, ErrorMessage = "Price must be greater than zero.")]
    public decimal Amount { get; set; } = 1.00m;

    [Required(ErrorMessage = "Currency is required.")]
    [StringLength(3, MinimumLength = 3, ErrorMessage = "Use a 3-letter currency code (e.g. USD).")]
    public string Currency { get; set; } = "USD";
}