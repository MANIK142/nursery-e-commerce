using Nursery.Catalog.Application.CareInstructions.Commands.Update;
using Nursery.Catalog.Application.Plants.Commands.UpdatePlant;

namespace Nursery.Catalog.Api.Endpoints.CareInstructions;

public class UpdateCareInstruction : ICarterModule
{
    public record UpdateCareInstructionRequest(Guid Id,
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
                                        string ModifiedBy);

    public record UpdateCareInstructionResponse(bool IsSuccess);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/careinstruction", async (UpdateCareInstructionRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateCareInstructionCommand>();
            var result = await sender.Send(command);
            return result.Adapt<UpdateCareInstructionResponse>();
        }).WithName("Update Care Instruction")
    .Produces<UpdateCareInstructionResponse>(StatusCodes.Status200OK)
    .ProducesProblem(StatusCodes.Status400BadRequest)
    .WithSummary("Update Care Instruction")
    .WithDescription("Update Care Instruction")
    .WithTags("Care Instructions");
    }
}
