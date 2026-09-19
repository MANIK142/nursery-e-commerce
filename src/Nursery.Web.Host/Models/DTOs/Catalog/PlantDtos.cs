using Nursery.Web.Host.Models.Enums;

namespace Nursery.Web.Host.Models.DTOs.Catalog;
public record PlantsResponse(IReadOnlyList<PlantDto> Plants);

public record PlantDto(
    Guid Id,
    string Name,
    string Description,
    IReadOnlyList<PlantImageDto> PlantImages,
    IReadOnlyList<CategoryDto> Categories,
    IReadOnlyList<PlantVariantDto> PlantVariants,
    CareInstructionDto? CareInstruction
);

public record PlantImageDto(Guid Id, string? AltText, string StorageKey, bool IsPrimaryImage);

public record CategoryDto(Guid Id, string Name);

public record MoneyDto(decimal Amount, string Currency);

public record SalePriceDto(MoneyDto SalePrice, DateTime StartsAtUtc, DateTime EndsAtUtc,bool IsActive);

public class PlantVariantDto
{
    public Guid Id { get; set; }
    public Guid PlantId { get; set; }
    public string Sku { get; set; } = default!;
    public string VariantName { get; set; } = default!;
    public MoneyDto RetailPrice { get; set; } = default!;
    public MoneyDto WholeSalePrice { get; set; } = default!;
    public MoneyDto Price { get; set; } = default!;
    public List<PlantImageDto> Images { get; set; } = default!;
    public List<SalePriceDto> SalePrices { get; set; } = default!;
}


public record CareInstructionDto(
    Guid Id,
    Guid PlantId,
    WateringFrequency WateringFrequency,
    SunlightRequirement SunlightRequirement,
    SoilType SoilType,
    int MinTemperatureCelsius,
    int MaxTemperatureCelsius,
    HumidityLevel HumidityLevel,
    FertilizingFrequency FertilizingFrequency,
    CareDifficultyLevel DifficultyLevel,
    bool IsToxicToPets,
    string? PruningNotes,
    string? AdditionalNotes
);