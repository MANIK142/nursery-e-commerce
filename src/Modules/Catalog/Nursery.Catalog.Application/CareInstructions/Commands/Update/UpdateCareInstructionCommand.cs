
using FluentValidation;
using Nursery.Catalog.Application.CareInstructions.Commands.Create;
using Nursery.Catalog.Domain.Enums;

namespace Nursery.Catalog.Application.CareInstructions.Commands.Update;

public record UpdateCareInstructionCommand(
                                        Guid Id,
                                        Guid PlantId,
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
                                        string? AdditionalNotes,
                                        string ModifiedBy) : ICommand<UpdateCareInstructionResult>;


public record UpdateCareInstructionResult(bool IsSuccess);


public class UpdateInstructionValidator : AbstractValidator<UpdateCareInstructionCommand>
{
    private const int MinPlausibleTemperatureCelsius = -30;
    private const int MaxPlausibleTemperatureCelsius = 55;
    private const int NotesMaxLength = 1000;
    public UpdateInstructionValidator()
    {
        RuleFor(x => x.PlantId)
            .NotEmpty().WithMessage("Plant Id can't be empty.");

        RuleFor(x => x.WateringFrequency)
            .NotEmpty().WithMessage("Watering frequency is required.")
            .Must(BeValidEnum<WateringFrequency>)
            .WithMessage(x => $"'{x.WateringFrequency}' is not a valid watering frequency. " +
                              $"Valid values: {ValidValues<WateringFrequency>()}.");

        RuleFor(x => x.SunlightRequirement)
            .NotEmpty().WithMessage("Sunlight requirement is required.")
            .Must(BeValidEnum<SunlightRequirement>)
            .WithMessage(x => $"'{x.SunlightRequirement}' is not a valid sunlight requirement. " +
                              $"Valid values: {ValidValues<SunlightRequirement>()}.");

        RuleFor(x => x.SoilType)
            .NotEmpty().WithMessage("Soil type is required.")
            .Must(BeValidEnum<SoilType>)
            .WithMessage(x => $"'{x.SoilType}' is not a valid soil type. " +
                              $"Valid values: {ValidValues<SoilType>()}.");

        RuleFor(x => x.HumidityLevel)
            .NotEmpty().WithMessage("Humidity level is required.")
            .Must(BeValidEnum<HumidityLevel>)
            .WithMessage(x => $"'{x.HumidityLevel}' is not a valid humidity level. " +
                              $"Valid values: {ValidValues<HumidityLevel>()}.");

        RuleFor(x => x.FertilizingFrequency)
            .NotEmpty().WithMessage("Fertilizing frequency is required.")
            .Must(BeValidEnum<FertilizingFrequency>)
            .WithMessage(x => $"'{x.FertilizingFrequency}' is not a valid fertilizing frequency. " +
                              $"Valid values: {ValidValues<FertilizingFrequency>()}.");

        RuleFor(x => x.DifficultyLevel)
            .NotEmpty().WithMessage("Difficulty level is required.")
            .Must(BeValidEnum<CareDifficultyLevel>)
            .WithMessage(x => $"'{x.DifficultyLevel}' is not a valid difficulty level. " +
                              $"Valid values: {ValidValues<CareDifficultyLevel>()}.");

        RuleFor(x => x.MinTemperatureCelsius)
            .InclusiveBetween(MinPlausibleTemperatureCelsius, MaxPlausibleTemperatureCelsius)
            .WithMessage($"Minimum temperature must be between {MinPlausibleTemperatureCelsius}°C " +
                         $"and {MaxPlausibleTemperatureCelsius}°C.");

        RuleFor(x => x.MaxTemperatureCelsius)
            .InclusiveBetween(MinPlausibleTemperatureCelsius, MaxPlausibleTemperatureCelsius)
            .WithMessage($"Maximum temperature must be between {MinPlausibleTemperatureCelsius}°C " +
                         $"and {MaxPlausibleTemperatureCelsius}°C.");

        RuleFor(x => x)
            .Must(x => x.MinTemperatureCelsius <= x.MaxTemperatureCelsius)
            .WithMessage("Minimum temperature cannot be greater than maximum temperature.")
            .WithName(nameof(CreateInstructionCommand.MinTemperatureCelsius));

        RuleFor(x => x.PruningNotes)
            .MaximumLength(NotesMaxLength)
            .WithMessage($"Pruning notes cannot exceed {NotesMaxLength} characters.");

        RuleFor(x => x.AdditionalNotes)
            .MaximumLength(NotesMaxLength)
            .WithMessage($"Additional notes cannot exceed {NotesMaxLength} characters.");

    }

    private static bool BeValidEnum<TEnum>(string value) where TEnum : struct, Enum =>
       Enum.TryParse<TEnum>(value, ignoreCase: true, out _);

    private static string ValidValues<TEnum>() where TEnum : struct, Enum =>
        string.Join(", ", Enum.GetNames<TEnum>());
}
