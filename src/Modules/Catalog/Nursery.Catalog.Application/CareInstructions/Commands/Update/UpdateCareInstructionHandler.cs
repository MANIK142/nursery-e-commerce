
using Nursery.Catalog.Application.Exceptions;
using Nursery.Catalog.Domain.Enums;
using Nursery.Catalog.Domain.Extenstions;
using System.Runtime.InteropServices;

namespace Nursery.Catalog.Application.CareInstructions.Commands.Update;

public class UpdateCareInstructionHandler(ICatalogRepository catalog) : ICommandHandler<UpdateCareInstructionCommand, UpdateCareInstructionResult>
{
    private readonly ICatalogRepository catalog = catalog;

    public async Task<UpdateCareInstructionResult> Handle(UpdateCareInstructionCommand request, CancellationToken cancellationToken)
    {
        var existingInstruction = await catalog.GetCareInstructionByPlantId(request.Id, cancellationToken);
        if (existingInstruction == null)
            throw new ItemNotFoundException($"Care Instructions not found for Plant Id {request.PlantId}");

        existingInstruction.UpdateSoilType(request.SoilType.ToEnum<SoilType>());
        existingInstruction.UpdateWatering(request.WateringFrequency.ToEnum<WateringFrequency>());
        existingInstruction.UpdateAdditionalNotes(request.AdditionalNotes);
        existingInstruction.UpdatePruningNotes(request.PruningNotes);
        existingInstruction.UpdateSunlight(request.SunlightRequirement.ToEnum<SunlightRequirement>());
        existingInstruction.UpdateFertilizingFrequency(request.FertilizingFrequency.ToEnum<FertilizingFrequency>());
        existingInstruction.UpdateDifficultyLevel(request.DifficultyLevel.ToEnum<CareDifficultyLevel>());
        existingInstruction.UpdateTemperatureRange(request.MinTemperatureCelsius,request.MaxTemperatureCelsius);

        var result = await catalog.UpdateCareInstructionByPlantId(existingInstruction, cancellationToken);
        return  new UpdateCareInstructionResult(result);
    }
}
