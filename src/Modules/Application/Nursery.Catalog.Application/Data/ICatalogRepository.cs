using Nursery.Catalog.Domain.Models;
namespace Nursery.Catalog.Application.Data;
public interface ICatalogRepository
{
    Task<List<Plant>> GetAllPlantsAsync(CancellationToken cancellationToken);
    Task<Plant> CreatePlantAsync(Plant plant, CancellationToken cancellationToken);
}
