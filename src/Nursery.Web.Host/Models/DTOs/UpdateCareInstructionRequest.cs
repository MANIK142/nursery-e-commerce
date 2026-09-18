using Nursery.Web.Host.Models.Enums;

namespace Nursery.Web.Host.Models.DTOs;

public record UpdateCareInstructionRequest
(
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
