
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Domain.Carts;
using Nursery.Orders.Domain.Orders;
using System.Net;

namespace Nursery.Orders.Application.Data;
public interface IOrdersDbContext
{
    DbSet<Cart> Carts { get; }
    DbSet<CartItem> CartItems { get; }

    DbSet<Order> Orders { get; }
    DbSet<OrderItem> OrderItems { get; }
}
