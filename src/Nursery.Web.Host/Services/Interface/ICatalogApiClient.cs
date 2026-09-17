using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.DTOs.Catalog;

namespace Nursery.Web.Host.Services.Interface;

public interface ICatalogApiClient
{

    Task<(bool IsSuccess, string? ErrorMessage)> CreatePlantAsync(CreatePlantApiRequest payload, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlantDto>> GetPlantsAsync(CancellationToken ct = default);
    Task<PlantDto?> GetPlantByIdAsync(Guid id, CancellationToken ct = default);

    Task<IEnumerable<CategoryDto>?> GetCatgoriesAsync(CancellationToken ct = default);
}