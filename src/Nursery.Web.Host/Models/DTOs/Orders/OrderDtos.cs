using Nursery.Web.Host.Models.DTOs.Customer;

namespace Nursery.Web.Host.Models.DTOs.Orders;

public static class OrderStatuses
{
    public const string PendingPayment = "Pending";
    public const string Paid = "Paid";
    public const string PaymentFailed = "Failed";
    public const string Cancelled = "Cancelled";
}

public sealed class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string OrderNumber { get; set; } = "";
    public string Status { get; set; } = "";
    public string PaymentStatus { get; set; } = "";
    public decimal Subtotal { get; set; }
    public decimal ShippingFee { get; set; }
    public decimal Total { get; set; }

    // Navigation / Related data
    public List<OrderLineDto> OrderItems { get; set; } = [];
    public AddressDto? ShippingAddress { get; set; }
}


public record FlatOrderDto(Guid Id, Guid CustomerId, string Status, string PaymentStatus, decimal Total, string OrderItems);

public static class OrderDtoExtention
{
    public static FlatOrderDto ToFlatOrderDto(this OrderDto orderDto)
    {
        return new FlatOrderDto(
            orderDto.Id,
            orderDto.CustomerId,
            orderDto.Status,
            orderDto.PaymentStatus,
            orderDto.OrderItems.Sum(i => i.LineTotal),
            string.Join(", ", orderDto.OrderItems.Select(i => i.ProductNameAtPurchase))
            );
    }
}







public sealed class OrderLineDto
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid PlantVariantId { get; set; }
    public string ProductNameAtPurchase { get; set; } = "";
    public decimal UnitPriceAtPurchase { get; set; }
    public int Quantity { get; set; }

    // Calculated / Display helpers
    public decimal LineTotal => UnitPriceAtPurchase * Quantity;
    public string? PlantName { get; set; }
    public string? VariantName { get; set; }
    public string? Sku { get; set; }
}

    public sealed class InitiatePaymentResponse
{
    public Guid PaymentId { get; set; }
    public string ClientSecret { get; set; } = "";
}

public sealed class StripeOptions
{
    public string PublishableKey { get; set; } = "";
}


