using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Domain.Models;
using Nursery.Catalog.Application.Data;

namespace Nursery.Catalog.Infrastructure.Persistence.Context;

public class CatalogDbContext : DbContext, ICatalogDbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }
    public DbSet<Plant> Plants => Set<Plant>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<PlantCategory> PlantCategories => Set<PlantCategory>();

    public DbSet<PlantVariant> PlantVariants => Set<PlantVariant>();
    public DbSet<VariantSalePrice> VariantSalePrices => Set<VariantSalePrice>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
