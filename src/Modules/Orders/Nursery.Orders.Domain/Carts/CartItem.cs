
using BuildingBlocks.Common;
using Nursery.Orders.Domain.Enums;

namespace Nursery.Orders.Domain.Carts;
public class CartItem : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid CartId { get; private set; }
    public Guid PlantvariantId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    private CartItem() { }
    private CartItem(Guid id, Guid cartId, Guid plantvariantId, int quantity, decimal unitPrice)
    {
        Id = id;
        CartId = cartId;
        PlantvariantId = plantvariantId;
        Quantity = quantity;
        UnitPrice = unitPrice;

    }

    internal static CartItem Create(Guid cartId, Guid plantvariantId, int quantity, decimal unitPrice)
    {
        var cartItem = new CartItem(Guid.NewGuid(),cartId,plantvariantId,quantity,unitPrice);
        return cartItem;
    }

    internal void AddQuantity() {
        Quantity ++;
    }
    internal void RemoveQuantity()
    {
        Quantity--;
    }

}
