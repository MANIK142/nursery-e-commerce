using BuildingBlocks.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Infrastructure.Persistence.Context;

public class CatalogDbContext : DbContext, ICatalogDbContext
{
    private readonly IPublisher _publisher;

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options,IPublisher publisher) : base(options)
    {
        this._publisher = publisher;
    }
    public DbSet<Plant> Plants => Set<Plant>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<PlantCategory> PlantCategories => Set<PlantCategory>();
    public DbSet<PlantVariant> PlantVariants => Set<PlantVariant>();
    public DbSet<VariantSalePrice> VariantSalePrices => Set<VariantSalePrice>();
    public DbSet<PlantImage> PlantImages => Set<PlantImage>();
    public DbSet<CareInstruction> CareInstructions => Set<CareInstruction>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
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
