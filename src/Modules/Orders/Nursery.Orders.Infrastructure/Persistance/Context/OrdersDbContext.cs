using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Domain.Carts;
using Nursery.Orders.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Infrastructure.Persistance.Context;

public class OrdersDbContext : DbContext, IOrdersDbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options):base(options)
    {
        
    }
    public DbSet<Cart> Carts => Set<Cart>();

    public DbSet<CartItem> CartItems => Set<CartItem>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrdersDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
