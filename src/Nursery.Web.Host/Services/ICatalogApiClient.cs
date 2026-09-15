using Nursery.Web.Host.Models.Catalog;

namespace Nursery.Web.Host.Services;

public interface ICatalogApiClient
{
    Task<PagedResult<PlantSummaryDto>> GetPlantsAsync(int page = 0, int pageSize = 3, CancellationToken ct = default);
    Task<PlantDetailDto?> GetPlantByIdAsync(Guid id, CancellationToken ct = default);
}