using BuildingBlocks.Common;
using MediatR;
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
    private readonly IPublisher _publisher;

    public OrdersDbContext(DbContextOptions<OrdersDbContext> options, IPublisher publisher):base(options)
    {
        this._publisher = publisher;
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

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = ChangeTracker.Entries<BaseDomainModel>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();
            foreach (var domainEvent in events)
                await _publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
