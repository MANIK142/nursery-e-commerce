
using BuildingBlocks.Common;
using Nursery.Orders.Domain.Enums;


namespace Nursery.Orders.Domain.Carts;
public class Cart : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    private readonly List<CartItem> _items = new();
    public IReadOnlyCollection<CartItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => _items.Sum(i => i.Quantity * i.UnitPrice);
    public CartStatus CartStatus { get; private set; }
    private Cart(){}
    private Cart(Guid id, Guid customerId , CartStatus cartStatus)
    {
        Id = id; 
        CustomerId = customerId;
        CartStatus = cartStatus;
    }
    public static Cart Create(Guid customerId)
    {
        var cart = new Cart(Guid.NewGuid(),customerId,CartStatus.Active);
        return cart;
    }

    public void AddcartItem(Guid PlantvariantId,decimal UnitPrice) {
        var cartItem = _items.FirstOrDefault(i => i.PlantvariantId == PlantvariantId);
        if (cartItem == null)
        {
             cartItem = CartItem.Create(Id, PlantvariantId, 1, UnitPrice);
            _items.Add(cartItem);
        }
    }

    public void RemoveItemFromCart(Guid PlantvariantId)
    {
        var cartItem = _items.FirstOrDefault(i => i.PlantvariantId == PlantvariantId);
        if (cartItem != null)
        {
            _items.Remove(cartItem);
        }
    }
    public void IncreaseItemQuantity(Guid PlantvariantId) 
    { 
        var cartItem = _items.FirstOrDefault(i => i.PlantvariantId == PlantvariantId);
        if (cartItem != null) {
            cartItem.AddQuantity();
        }
    }
    public void DecreaseItemQuantity(Guid PlantvariantId)
    {
        var cartItem = _items.FirstOrDefault(i => i.PlantvariantId == PlantvariantId);
        if (cartItem != null)
        {
            if (cartItem.Quantity > 0) {
                cartItem.RemoveQuantity();
            }  
        }
    }


}
