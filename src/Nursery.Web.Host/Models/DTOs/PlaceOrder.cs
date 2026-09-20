using Nursery.Web.Host.Models.DTOs.Customer;

namespace Nursery.Web.Host.Models.DTOs;

public class PlaceOrderRequest
{
    public Guid customerId { get; set; }
    public AddressDto billingAddress { get; set; }
    public AddressDto shippingAddress { get; set; }
    public List<PlaceOrderItem> items { get; set; }
}

public class PlaceOrderItem
{
    public Guid plantVariantId { get; set; }
    public int quantity { get; set; }
}


public sealed class PlaceOrderResponse
{
    public Guid OrderId { get; set; }
}