using Nursery.Web.Host.Models.DTOs;

namespace Nursery.Web.Host.Models.ViewModels.Cart;

public class CartViewModel
{
    public List<CartItemsViewModel> Items { get; set; }
    public decimal TotalPice => Items.Sum(i => i.ItemTotalPrice);
}

public class CartItemsViewModel
{
    public Guid PlantVariantId { get; set; }
    public string PlantVariantName { get; set; }
    public string Sku { get; set; }
    public Decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal ItemTotalPrice => Quantity * Price;   
 }
