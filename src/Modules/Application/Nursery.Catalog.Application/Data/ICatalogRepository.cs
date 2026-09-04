using Nursery.Catalog.Domain.Models;
namespace Nursery.Catalog.Application.Data;
public interface ICatalogRepository
{
    Task<bool> IsPlantExistsAsync(string name, CancellationToken cancellationToken);
    Task<Plant?> GetPlantById(Guid Id, CancellationToken cancellationToken);
    Task<Plant> CreatePlantAsync(Plant plant, CancellationToken cancellationToken);
    Task<bool> UpdatePlantAsync(Plant plant, CancellationToken cancellationToken);
    Task<bool> DeletePlantById(Guid Id, CancellationToken cancellationToken);


    Task<bool> IsCategoryExistsAsync(string name, CancellationToken cancellationToken);
    Task<Category?> GetCategoryById(Guid Id, CancellationToken cancellationToken);
    Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<bool> UpdateCategoryAsync(Category category, CancellationToken cancellationToken);
    Task<bool> DeleteCategoryById(Guid Id, CancellationToken cancellationToken);
}
