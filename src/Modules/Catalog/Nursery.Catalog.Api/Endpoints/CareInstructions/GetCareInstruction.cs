using Nursery.Catalog.Application.CareInstructions.Queries.GetCareInstructions;
using Nursery.Catalog.Application.Dtos;

namespace Nursery.Catalog.Api.Endpoints.CareInstructions;

public class GetCareInstruction:ICarterModule
{
    public record GetCareInstructionRequest(int? PageNumber, int? PageSize, Guid? Id, string? FilterBy, string? FilterValue);
    public record GetCareInstructionResponse(IEnumerable<CareInstructionDto> CareInstruction);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/careinstruction", async ([AsParameters] GetCareInstructionRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetCareInstructionQuery>();
            var response = await sender.Send(query, CancellationToken.None);
            return Results.Ok(response.Adapt<GetCareInstructionResponse>());
        }).WithName("Get Care Instructions")
        .Produces<GetCareInstructionResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Care Instructions")
        .WithDescription("Get Care Instructions")
        .WithTags("Care Instructions");
    }
}
