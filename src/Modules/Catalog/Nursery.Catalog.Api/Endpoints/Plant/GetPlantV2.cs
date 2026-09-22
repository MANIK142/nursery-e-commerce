using Nursery.Catalog.Application.Dtos;
using Nursery.Catalog.Application.Plants.Queries.GetPlants;

namespace Nursery.Catalog.Api.Endpoints.Plant;

public class GetPlantV2 : ICarterModule
{
    public record GetPlantRequest(int? PageNumber, int? PageSize, Guid? Id, string? FilterBy, string? FilterValue);
    public record GetPlantResponse(IEnumerable<PlantDto> Plants);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v{version:apiVersion}/plants", async ([AsParameters] GetPlantRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetCareInstructionQuery>();
            var response = await sender.Send(query, CancellationToken.None);
            return Results.Ok(response.Adapt<GetPlantResponse>());
        })
        .WithApiVersionSet(PlantApiVersioning.VersionSet(app))
        .MapToApiVersion(2.0)
        .WithName("Get Plants v2")
        .Produces<GetPlantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Plants v2")
        .WithDescription("Get Plants v2")
        .WithTags("Plants");
        //.RequireAuthorization();
    }
}   
