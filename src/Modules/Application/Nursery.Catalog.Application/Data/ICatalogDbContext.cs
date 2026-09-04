
using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Application.Data;

public interface ICatalogDbContext
{
    DbSet<Plant> Plants { get; }
    DbSet<Category> Categories { get; }
    DbSet<PlantCategory> PlantCategories { get; }
}
