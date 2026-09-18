
using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Application.Dtos;
using Nursery.Catalog.Domain.Models;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using System.ComponentModel.DataAnnotations;
namespace Nursery.Catalog.Infrastructure.Repository;
public class CatalogRepository(CatalogDbContext _db) : ICatalogRepository
{
    private readonly CatalogDbContext db = _db;
    public async Task<bool> IsPlantExistsAsync(string name, CancellationToken cancellationToken)
    {
        var plant = await db.Plants.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);
        if (plant == null)
            return false;
        return true;
    }
    public async Task<Plant?> GetPlantById(Guid Id, CancellationToken cancellationToken)
    {
        var plant = await db.Plants
                        .Include(p => p.Categories)
                        .Include(p => p.Images) 
                        .Include(p => p.Variants)
                            .ThenInclude(v => v.SalePrices)
                        .Include(p => p.Variants)
                            .ThenInclude(v => v.Images) 
                        .FirstOrDefaultAsync(p => p.Id == Id, cancellationToken);
        return plant;
    }

    public async Task<List<Guid>?> GetCategoriesByPlantId(Guid PlantId, CancellationToken cancellationToken)
    {
        return await db.PlantCategories
                .Where(pc => pc.PlantId == PlantId)
                .Select(pc => pc.CategoryId).ToListAsync();
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
        var category = await db.Categories.FirstOrDefaultAsync(p => p.Name == name,cancellationToken);
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
    public async Task<CareInstruction> CreateCareInstruction(CareInstruction instruction, CancellationToken cancellationToken)
    {
        db.CareInstructions.Add(instruction);
        await db.SaveChangesAsync(cancellationToken);
        return instruction;
    }



    public async Task<CareInstruction?> GetCareInstructionByPlantId(Guid Id, CancellationToken cancellationToken)
    {
        var careInstruction = await db.CareInstructions.FirstOrDefaultAsync(c => c.PlantId == Id, cancellationToken);
        return careInstruction;
    }

    public async Task<bool> UpdateCareInstructionByPlantId(CareInstruction instruction, CancellationToken cancellationToken)
    {
        var affectedRows = await db.SaveChangesAsync(cancellationToken);
        return affectedRows > 0;
    }

    public async Task<bool> DeleteCareInstructionByPlantId(Guid Id, CancellationToken cancellationToken)
    {
        var affectedRows = await db.CareInstructions.Where(c => c.PlantId == Id).ExecuteDeleteAsync(cancellationToken);
        return affectedRows > 0;
    }
}
