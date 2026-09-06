
using Nursery.Catalog.Domain.Enums;

namespace Nursery.Catalog.Domain.Models;
public class CareInstruction :BaseDomainModel
{
    public Guid Id { get; private set; } 
    public Guid PlantId { get; private set; }
    public WateringFrequency WateringFrequency { get; private set; }
    public SunlightRequirement SunlightRequirement { get; private set; }
    public SoilType SoilType { get; private set; }
    public int MinTemperatureCelsius { get; private set; }
    public int MaxTemperatureCelsius { get; private set; }
    public HumidityLevel HumidityLevel { get; private set; }
    public FertilizingFrequency FertilizingFrequency { get; private set; }
    public CareDifficultyLevel DifficultyLevel { get; private set; }
    public bool IsToxicToPets { get; private set; }
    public string? PruningNotes { get; private set; }
    public string? AdditionalNotes { get; private set; }

    private CareInstruction() { } // EF Core

    private CareInstruction(
        Guid id,
        Guid plantId,
        WateringFrequency wateringFrequency,
        SunlightRequirement sunlightRequirement,
        SoilType soilType,
        int minTemperatureCelsius,
        int maxTemperatureCelsius,
        HumidityLevel humidityLevel,
        FertilizingFrequency fertilizingFrequency,
        CareDifficultyLevel difficultyLevel,
        bool isToxicToPets) 
    {
        PlantId = plantId;
        WateringFrequency = wateringFrequency;
        SunlightRequirement = sunlightRequirement;
        SoilType = soilType;
        SetTemperatureRange(minTemperatureCelsius, maxTemperatureCelsius);
        HumidityLevel = humidityLevel;
        FertilizingFrequency = fertilizingFrequency;
        DifficultyLevel = difficultyLevel;
        IsToxicToPets = isToxicToPets;
    }

    public static CareInstruction Create(
        Guid plantId,
        WateringFrequency wateringFrequency,
        SunlightRequirement sunlightRequirement,
        SoilType soilType,
        int minTemperatureCelsius,
        int maxTemperatureCelsius,
        HumidityLevel humidityLevel,
        FertilizingFrequency fertilizingFrequency,
        CareDifficultyLevel difficultyLevel,
        bool isToxicToPets)
    {
        

        var careInstruction = new CareInstruction(
            Guid.NewGuid(),
            plantId,
            wateringFrequency,
            sunlightRequirement,
            soilType,
            minTemperatureCelsius,
            maxTemperatureCelsius,
            humidityLevel,
            fertilizingFrequency,
            difficultyLevel,
            isToxicToPets);

        

        return careInstruction;
    }

    public void UpdateWatering(WateringFrequency frequency)
    {
        if (WateringFrequency == frequency) return;

        WateringFrequency = frequency;

    }

    public void UpdateSunlight(SunlightRequirement sunlight)
    {
        if (SunlightRequirement == sunlight) return;

        SunlightRequirement = sunlight;
 
    }

    public void UpdateSoilType(SoilType soilType)
    {
        if (SoilType == soilType) return;

        SoilType = soilType;
    }

    public void UpdateTemperatureRange(int minCelsius, int maxCelsius)
    {
        if (MinTemperatureCelsius == minCelsius && MaxTemperatureCelsius == maxCelsius) return;

        SetTemperatureRange(minCelsius, maxCelsius);
    }

    public void UpdateHumidityLevel(HumidityLevel humidityLevel)
    {
        if (HumidityLevel == humidityLevel) return;

        HumidityLevel = humidityLevel;
    }

    public void UpdateFertilizingFrequency(FertilizingFrequency frequency)
    {
        if (FertilizingFrequency == frequency) return;

        FertilizingFrequency = frequency;
    }

    public void UpdateDifficultyLevel(CareDifficultyLevel level)
    {
        if (DifficultyLevel == level) return;

        DifficultyLevel = level;
    }

    public void SetToxicityToPets(bool isToxic)
    {
        if (IsToxicToPets == isToxic) return;

        IsToxicToPets = isToxic;
    }

    public void UpdatePruningNotes(string? notes)
    {
        PruningNotes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
    }

    public void UpdateAdditionalNotes(string? notes)
    {
        AdditionalNotes = string.IsNullOrWhiteSpace(notes) ? null : notes.Trim();
    }

    private void SetTemperatureRange(int minCelsius, int maxCelsius)
    {
        if (minCelsius > maxCelsius)
        {
            throw new ArgumentException(
                $"Minimum temperature ({minCelsius}°C) cannot exceed maximum ({maxCelsius}°C).");
        }

        MinTemperatureCelsius = minCelsius;
        MaxTemperatureCelsius = maxCelsius;
    }
}
