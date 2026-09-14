using BuildingBlocks.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Infrastucture.Presistance.Context;

public class ShippingDbContext :DbContext, IShippingDbContext
{
    private readonly IPublisher _publisher;

    public ShippingDbContext(DbContextOptions<ShippingDbContext> options,IPublisher publisher) :base(options)
    {
        this._publisher = publisher;
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();

    public DbSet<ShipmentLineItem> ShipmentLineItems => Set<ShipmentLineItem>();

    public DbSet<Return> Returns => Set<Return>();

    public DbSet<ReturnLineItem> ReturnLineItems => Set<ReturnLineItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ShippingDbContext).Assembly);
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
