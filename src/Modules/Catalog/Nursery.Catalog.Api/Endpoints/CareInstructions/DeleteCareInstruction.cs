using Nursery.Catalog.Application.CareInstructions.Commands.Delete;


namespace Nursery.Catalog.Api.Endpoints.CareInstructions;

public class DeleteCareInstruction : ICarterModule
{
    public record DeleteCareInstructionRequest(Guid PlantId);
    public record DeleteCareInstructionResponse(bool IsSuccess);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/careinstruction/{PlantId:guid}", async ([AsParameters] DeleteCareInstructionRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteCareInstructionCommand>();
            var result = await sender.Send(command);
            return result.Adapt<DeleteCareInstructionResponse>();
        }).WithName("Delete Care Instruction")
        .Produces<DeleteCareInstruction>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Care Instruction")
        .WithDescription("Delete Care Instruction")
        .WithTags("Care Instructions");
    }
}
