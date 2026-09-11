using BuildingBlocks.Common;
using System.Reflection.Emit;

namespace Nursery.Orders.Domain.Orders;
public class OrderItem : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public Guid PlantVariantId { get; private set; }
    public string ProductNameAtPurchase { get; set; } = default!;
    public int Quantity { get; private set; }
    public decimal UnitPriceAtPurchase { get; private set; }

    public decimal LineTotal => UnitPriceAtPurchase * Quantity;

    private OrderItem() { } // EF Core

    internal static OrderItem Create(
        Guid orderId,
        Guid plantVariantId,
        string productNameAtPurchase,
        decimal unitPriceAtPurchase,
        int quantity)
    {
        if(quantity <= 0)
        {
            throw new ArgumentOutOfRangeException("Item Quantity cannot be Zero or Less than zero");
        }
        if (unitPriceAtPurchase <= 0)
        {
            throw new ArgumentOutOfRangeException("Price cannot be Zero or Less than zero");
        }

        return new OrderItem
        {
            Id = Guid.NewGuid(),
            OrderId = orderId,
            PlantVariantId = plantVariantId,
            ProductNameAtPurchase = productNameAtPurchase,
            UnitPriceAtPurchase = unitPriceAtPurchase,
            Quantity = quantity
        };
    }

    internal void ChangeQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException("Item Quantity cannot be Zero or Less than zero");
        }
        Quantity = quantity;
    }
}
