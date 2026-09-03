using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Infrastructure.Persistence.Context;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
    }
    public DbSet<Plant> Plants => Set<Plant>();
    public DbSet<Category> Categories => Set<Category>();
    //public DbSet<PlantCategory> PlantCategories => Set<PlantCategory>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
