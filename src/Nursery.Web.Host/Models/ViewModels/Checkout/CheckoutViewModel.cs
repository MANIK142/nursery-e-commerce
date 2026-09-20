using System.ComponentModel.DataAnnotations;

namespace Nursery.Web.Host.Models.ViewModels.Checkout;

public class CheckoutViewModel
{
    public List<CheckoutItemViewModel> Items { get; set; } = new();

    public decimal SubTotal => Items.Sum(i => i.TotalPrice);
    public decimal ShippingFee { get; set; } = 15.00m;
    public decimal EstimatedTax => SubTotal * 0.08m; // 8% sales tax
    public decimal OrderTotal => SubTotal + ShippingFee + EstimatedTax;

    // Customer Identity
    [Required(ErrorMessage = "Customer ID is required.")]
    public string CustomerId { get; set; } = string.Empty;

    // Address Sections matching POST /api/v1/orders
    public CheckoutAddressInputModel BillingAddress { get; set; } = new();

    public CheckoutAddressInputModel ShippingAddress { get; set; } = new();

    // UI Toggle: Copy billing details to shipping
    public bool SameAsBilling { get; set; } = true;

    [Required]
    public string PaymentMethod { get; set; } = "CreditCard";

}


public class CheckoutAddressInputModel
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string? EmailAddress { get; set; }

    [Required(ErrorMessage = "Street address is required.")]
    [StringLength(200)]
    public string AddressLine { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(50)]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "Postal/Zip code is required.")]
    [StringLength(20)]
    public string ZipCode { get; set; } = string.Empty;
}

public class CheckoutItemViewModel
{
    [Required]
    public Guid PlantVariantId { get; set; }

    public string PlantName { get; set; } = string.Empty;
    public string VariantName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal UnitPrice { get; set; }

    [Range(1, 1000, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; } = 1;

    public decimal TotalPrice => UnitPrice * Quantity;
    public string? ImageUrl { get; set; }
}