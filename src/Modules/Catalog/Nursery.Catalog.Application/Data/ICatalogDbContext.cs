
using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Application.Data;

public interface ICatalogDbContext
{
    DbSet<Plant> Plants { get; }
    DbSet<Category> Categories { get; }
    DbSet<PlantCategory> PlantCategories { get; }
    DbSet<PlantVariant> PlantVariants { get; }
    DbSet<VariantSalePrice> VariantSalePrices { get; }
    DbSet<PlantImage> PlantImages { get; }
    DbSet<CareInstruction> CareInstructions { get; }
}
