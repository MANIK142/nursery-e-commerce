using Nursery.Web.Host.Models.Catalog;

namespace Nursery.Web.Host.Services.Interface;

public interface ICatalogApiClient
{
    Task<IReadOnlyList<PlantDto>> GetPlantsAsync(CancellationToken ct = default);
    Task<PlantDto?> GetPlantByIdAsync(Guid id, CancellationToken ct = default);

    Task<IEnumerable<CategoryDto>?> GetCatgoriesAsync(CancellationToken ct = default);
}