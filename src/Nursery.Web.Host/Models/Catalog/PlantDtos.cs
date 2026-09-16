namespace Nursery.Web.Host.Models.Catalog;
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

public record SalePriceDto(MoneyDto SalePrice, DateTime StartsAtUtc, DateTime EndsAtUtc);

public class PlantVariantDto
{
    public Guid Id { get; set; }
    public Guid PlantId { get; set; }
    public string Sku { get; set; } = default!;
    public string VariantName { get; set; } = default!;
    public MoneyDto RetailPrice { get; set; } = default!;
    public MoneyDto Price { get; set; } = default!;
    public List<PlantImageDto> Images { get; set; } = default!;
}


public record CareInstructionDto(
    string WateringFrequency,
    string SunlightRequirement,
    string SoilType,
    int MinTemperatureCelsius,
    int MaxTemperatureCelsius,
    string HumidityLevel,
    string FertilizingFrequency,
    string DifficultyLevel,
    bool IsToxicToPets,
    string? PruningNotes,
    string? AdditionalNotes
);