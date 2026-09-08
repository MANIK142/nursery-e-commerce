

using System.Text.Json.Serialization;

namespace Nursery.Catalog.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WateringFrequency
{
    Daily,
    TwiceAWeek,
    Weekly,
    BiWeekly,
    Monthly,
    WhenSoilIsDry
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SunlightRequirement
{
    FullSun,
    PartialSun,
    PartialShade,
    FullShade
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SoilType
{
    WellDraining,
    Sandy,
    Loamy,
    Clay,
    Peaty,
    Chalky,
    Succulent  // cactus/succulent mix
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum HumidityLevel
{
    Low,
    Medium,
    High
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FertilizingFrequency
{
    Weekly,
    BiWeekly,
    Monthly,
    Seasonally,
    Rarely,
    NotRequired
}
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CareDifficultyLevel
{
    Beginner,
    Intermediate,
    Expert
}
