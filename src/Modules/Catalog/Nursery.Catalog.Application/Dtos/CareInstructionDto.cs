
using Nursery.Catalog.Domain.Enums;

namespace Nursery.Catalog.Application.Dtos;
public class CareInstructionDto
{
    public Guid? Id { get; set; }
    public Guid PlantId { get;  set; }
    public WateringFrequency WateringFrequency { get;  set; }
    public SunlightRequirement SunlightRequirement { get;  set; }
    public SoilType SoilType { get;  set; }
    public int MinTemperatureCelsius { get;  set; }
    public int MaxTemperatureCelsius { get;  set; }
    public HumidityLevel HumidityLevel { get;  set; }
    public FertilizingFrequency FertilizingFrequency { get;  set; }
    public CareDifficultyLevel DifficultyLevel { get;  set; }
    public bool IsToxicToPets { get; set; }
    public string? PruningNotes { get; set; }
    public string? AdditionalNotes { get; set; }
}
