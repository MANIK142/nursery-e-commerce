
using BuildingBlocks.Common;
using Nursery.Orders.Domain.Enums;
using Nursery.Orders.Domain.ValueObjects;
namespace Nursery.Orders.Domain.Orders;
public class Order : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    private List<OrderItem> _orderItems = [];
    public IEnumerable<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public Address BillingAddress { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!;
    public OrderStatus Status { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }

    public static Order Create(Guid CustomerId,Address BillingAddress, Address ShippingAddress)
    {
        return new Order
        {
            Id = Guid.NewGuid(),
            CustomerId = CustomerId,
            BillingAddress = BillingAddress,
            ShippingAddress = ShippingAddress,
            Status = OrderStatus.Pending,
            PaymentStatus = PaymentStatus.Pending
        };
    }

    public void AddItem(Guid plantVariantId, string productName, decimal unitPrice, int quantity)
    {
        EnsureMutable();
        var existing = _orderItems.SingleOrDefault(i => i.PlantVariantId == plantVariantId);
        if (existing is not null)
        {
            existing.ChangeQuantity(existing.Quantity + quantity);
            return;
        }

        _orderItems.Add(OrderItem.Create(Id, plantVariantId, productName, unitPrice, quantity));
    }

    public void RemoveItem(Guid orderItemId)
    {
        EnsureMutable();
        var item = _orderItems.SingleOrDefault(i => i.Id == orderItemId)
            ?? throw new InvalidOperationException("Order item not found.");
        _orderItems.Remove(item);
    }

    public void PlaceOrder()
    {
        Status = OrderStatus.Confirmed;
    }

    public void CancelOrder()
    {
        Status = OrderStatus.Cancelled;
    }
    private void EnsureMutable()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException($"Cannot modify an order in status '{Status}'.");
    }
}
