
using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Domain.Models;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using System.Runtime.InteropServices;

namespace Nursery.Catalog.Infrastructure.Repository;

public class CatalogRepository(CatalogDbContext _db) : ICatalogRepository
{
    private readonly CatalogDbContext db = _db;
    public async Task<bool> IsPlantExistsAsync(string name, CancellationToken cancellationToken)
    {
        var plant = await db.Plants.FirstOrDefaultAsync(p => p.Name == name);
        if (plant == null)
            return false;
        return true;
    }

    public async Task<Plant?> GetPlantById(Guid Id, CancellationToken cancellationToken)
    {
        var plant = await db.Plants.FirstOrDefaultAsync(p => p.Id == Id, cancellationToken);
        return plant;
    }

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
    public async Task<bool> UpdatePlantAsync(Plant plant, CancellationToken cancellationToken)
    {
        db.Plants.Update(plant);
        var affectedRows = await db.SaveChangesAsync(cancellationToken);
        return affectedRows > 0;
    }

    public async Task<bool> DeletePlantById(Guid Id, CancellationToken cancellationToken)
    {
        var affectedRows = await db.Plants.Where(p => p.Id == Id).ExecuteDeleteAsync(cancellationToken);
        return affectedRows > 0;
    }



    public async Task<bool> IsCategoryExistsAsync(string name, CancellationToken cancellationToken)
    {
        var category = await db.Categories.FirstOrDefaultAsync(p => p.Name == name);
        if (category == null)
            return false;
        return true;
    }

    public async Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        db.Categories.Add(category);
        var result = await db.SaveChangesAsync(cancellationToken);
        return category;

    }

    public async Task<bool> UpdateCategoryAsync(Category category, CancellationToken cancellationToken)
    {
        db.Categories.Update(category);
        var affectedRows = await db.SaveChangesAsync(cancellationToken);
        return affectedRows > 0;
    }



    public async Task<Category?> GetCategoryById(Guid Id, CancellationToken cancellationToken)
    {
        var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == Id, cancellationToken);
        return category;
    }

    public async Task<bool> DeleteCategoryById(Guid Id, CancellationToken cancellationToken)
    {
        var affectedRows = await db.Categories.Where(c => c.Id == Id).ExecuteDeleteAsync(cancellationToken);
        return affectedRows > 0;
    }
}
