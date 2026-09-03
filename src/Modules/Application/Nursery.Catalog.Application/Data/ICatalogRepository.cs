using Nursery.Catalog.Domain.Models;
namespace Nursery.Catalog.Application.Data;
public interface ICatalogRepository
{
    Task<bool> IsPlantExistsAsync(string name, CancellationToken cancellationToken);
    Task<Plant> CreatePlantAsync(Plant plant, CancellationToken cancellationToken);
    Task<bool> IsCategoryExistsAsync(string name, CancellationToken cancellationToken);
    Task<Category> CreateCategoryAsync(Category category, CancellationToken cancellationToken);
}
