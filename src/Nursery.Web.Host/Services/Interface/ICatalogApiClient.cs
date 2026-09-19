using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.DTOs.Catalog;

namespace Nursery.Web.Host.Services.Interface;

public interface ICatalogApiClient
{

    Task<(bool IsSuccess, string? ErrorMessage)> CreatePlantAsync(CreatePlantApiRequest payload, CancellationToken cancellationToken = default);
    Task<(bool IsSuccess, string? ErrorMessage)> UpdatePlantAsync(Guid id, UpdatePlantApiRequest payload, CancellationToken cancellationToken = default);

    Task<(bool IsSuccess, string? ErrorMessage)> AddToCardAsync(Guid plantVariantid, CancellationToken cancellationToken = default);

    Task<PlantDto?> GetPlantByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<PlantDto>> GetPlantsAsync(CancellationToken ct = default);

    Task<IEnumerable<CategoryDto>?> GetCatgoriesAsync(CancellationToken ct = default);

    Task<(bool IsSuccess, string? ErrorMessage)> CreateCareInstruction(CreateCareInstructionRequest payload, CancellationToken ct = default);
    Task<(bool IsSuccess, string? ErrorMessage)> UpdateCareInstruction(UpdateCareInstructionRequest payload, CancellationToken cancellationToken = default);

    Task<(bool IsSuccess, string? ErrorMessage)> DeletePlant(Guid PlantId, CancellationToken cancellationToken = default);
}