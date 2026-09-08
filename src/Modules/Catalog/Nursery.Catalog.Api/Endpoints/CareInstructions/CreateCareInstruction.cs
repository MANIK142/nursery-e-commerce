using BuildingBlocks.Common.CQRS;
using Nursery.Catalog.Application.CareInstructions.Commands.Create;

namespace Nursery.Catalog.Api.Endpoints.CareInstructions;

public record CreateInstructionRequest(Guid PlantId,
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
                                        string CreatedBy) : ICommand<CreateInstructionResponse>;
public record CreateInstructionResponse(Guid Id);
public class CreateCareInstruction : ICarterModule
{
    
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/careinstruction", async (CreateInstructionRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateInstructionCommand>();
            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<CreateInstructionResponse>());

        }).WithDisplayName("Create care instructions")
        .WithTags("Care Instructions")
        .WithSummary("Create care instructions")
        .Produces<CreateInstructionResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
