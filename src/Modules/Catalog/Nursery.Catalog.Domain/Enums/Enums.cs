

namespace Nursery.Catalog.Domain.Enums;
public enum WateringFrequency
{
    Daily,
    TwiceAWeek,
    Weekly,
    BiWeekly,
    Monthly,
    WhenSoilIsDry
}

public enum SunlightRequirement
{
    FullSun,
    PartialSun,
    PartialShade,
    FullShade
}

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

public enum HumidityLevel
{
    Low,
    Medium,
    High
}

public enum FertilizingFrequency
{
    Weekly,
    BiWeekly,
    Monthly,
    Seasonally,
    Rarely,
    NotRequired
}

public enum CareDifficultyLevel
{
    Beginner,
    Intermediate,
    Expert
}
