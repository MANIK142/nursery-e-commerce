namespace Nursery.Web.Host.Models.ViewModels.Orders;

public class OrderDetailsViewModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public List<OrderItemViewModel> OrderItems { get; set; } = new();

    // Calculated properties for UI display
    public decimal TotalAmount => OrderItems.Sum(x => x.TotalPrice);
    public int TotalItemCount => OrderItems.Sum(x => x.Quantity);
}

public class OrderItemViewModel
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid PlantVariantId { get; set; }
    public string ProductNameAtPurchase { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPriceAtPurchase { get; set; }

    // Line total
    public decimal TotalPrice => Quantity * UnitPriceAtPurchase;
}