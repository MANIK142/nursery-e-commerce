using System.ComponentModel.DataAnnotations;

namespace Nursery.Web.Host.Models.ViewModels.Orders;

public class CreateOrderViewModel
{
    [Required(ErrorMessage = "Customer ID is required.")]
    public string CustomerId { get; set; } = string.Empty;

    public AddressInputModel BillingAddress { get; set; } = new();

    public AddressInputModel ShippingAddress { get; set; } = new();

    [Required(ErrorMessage = "Order must contain at least one item.")]
    [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
    public List<OrderItemInputModel> Items { get; set; } = new();

    // Helper flag often used in checkout forms
    public bool SameAsBilling { get; set; } = true;
}

public class AddressInputModel
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? EmailAddress { get; set; }

    [Required(ErrorMessage = "Address line is required.")]
    [StringLength(200)]
    public string AddressLine { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(50)]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "Zip/Postal code is required.")]
    [StringLength(20)]
    public string ZipCode { get; set; } = string.Empty;
}

public class OrderItemInputModel
{
    [Required(ErrorMessage = "Plant variant ID is required.")]
    public Guid PlantVariantId { get; set; }

    [Range(1, 1000, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; } = 1;
}