using Nursery.Catalog.Application.Exceptions;
using Nursery.Catalog.Domain.Enums;
using Nursery.Catalog.Domain.Extenstions;

namespace Nursery.Catalog.Application.CareInstructions.Commands.Create;

public class CreateInstructionHandler(ICatalogRepository catalog) : ICommandHandler<CreateInstructionCommand, CreateInstructionResult>
{
    private readonly ICatalogRepository catalog = catalog;

    public async Task<CreateInstructionResult> Handle(CreateInstructionCommand request, CancellationToken cancellationToken)
    {

        var existingItem = await catalog.GetCareInstructionByPlantId(request.PlantId,cancellationToken);
        if (existingItem != null)
            throw new ItemAlreadyExistsException($"Care Instruction already available for Plant Id {request.PlantId}");
            

        var careInstruction = CareInstruction.Create(request.PlantId, request.WateringFrequency.ToEnum<WateringFrequency>(),
            request.SunlightRequirement.ToEnum<SunlightRequirement>(), request.SoilType.ToEnum<SoilType>(), 
            request.MinTemperatureCelsius, request.MaxTemperatureCelsius,
            request.HumidityLevel.ToEnum<HumidityLevel>(), request.FertilizingFrequency.ToEnum<FertilizingFrequency>(), 
            request.DifficultyLevel.ToEnum<CareDifficultyLevel>(), request.IsToxicToPets, 
            request.AdditionalNotes, request.PruningNotes,request.Createdby);


        var result = await catalog.CreateCareInstruction(careInstruction,cancellationToken);

        return new CreateInstructionResult(result.Id);
    }

   
}
