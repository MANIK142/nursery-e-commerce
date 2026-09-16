using Nursery.Web.Host.Models.Catalog;

namespace Nursery.Web.Host.Services;

public interface ICatalogApiClient
{
    Task<IReadOnlyList<PlantDto>> GetPlantsAsync(CancellationToken ct = default);
    Task<PlantDto?> GetPlantByIdAsync(Guid id, CancellationToken ct = default);
}