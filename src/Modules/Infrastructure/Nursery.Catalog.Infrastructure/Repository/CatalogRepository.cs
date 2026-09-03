
using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Domain.Models;
using Nursery.Catalog.Infrastructure.Persistence.Context;

namespace Nursery.Catalog.Infrastructure.Repository;

public class CatalogRepository(CatalogDbContext _db) : ICatalogRepository
{
    private readonly CatalogDbContext db = _db;

    public async Task<List<Plant>> GetAllPlantsAsync(CancellationToken cancellationToken)
    {
        return await db.Plants.ToListAsync(cancellationToken);
    }

    public async Task<Plant> CreatePlantAsync(Plant plant, CancellationToken cancellationToken)
    {
        var existingPlant = await db.Plants.FirstOrDefaultAsync(p => p.Name == plant.Name, cancellationToken);
        if (existingPlant == null)
        {
            db.Plants.Add(plant);
            await db.SaveChangesAsync(cancellationToken);
        }
        else
        {
            plant = existingPlant;
        }
        return plant;
    }
}
