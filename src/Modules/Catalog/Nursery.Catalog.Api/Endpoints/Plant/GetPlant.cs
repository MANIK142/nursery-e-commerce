using Nursery.Catalog.Application.Dtos;
using Nursery.Catalog.Application.Plants.Queries.GetPlants;

namespace Nursery.Catalog.Api.Endpoints.Plant;

public class GetPlant : ICarterModule
{
    public record GetPlantRequest(int? PageNumber, int? PageSize, Guid? Id, string? FilterBy, string? FilterValue);
    public record GetPlantResponse(IEnumerable<PlantDto> Plants);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/plants", async ([AsParameters] GetPlantRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetCareInstructionQuery>();
            var response = await sender.Send(query, CancellationToken.None);
            return Results.Ok(response.Adapt<GetPlantResponse>());
        }).WithName("Get Plants")
        .Produces<GetPlantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Plants")
        .WithDescription("Get Plants")
        .WithTags("Plants");
    }
}   
