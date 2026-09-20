
using Nursery.Catalog.Application.Dtos;
using Nursery.Catalog.Application.Plants.Queries.GetPlantVariants;


namespace Nursery.Catalog.Api.Endpoints.Plant;


public record GetPlantVariantsRequest(List<Guid> PlantVariantIds);
public record GetPlantVaraintsResponse(List<PlantVariantDto> PlantVariants);
public class GetPlantVariants : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/plantvariants", async (GetPlantVariantsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetPlantVariantsQuery>();
            var response = await sender.Send(query, CancellationToken.None);
            return Results.Ok(response.Adapt<GetPlantVaraintsResponse>());

        }).WithName("Get Plant Variants")
        .Produces<GetPlantVaraintsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Plant Variants")
        .WithDescription("Get Plant Variants")
        .WithTags("Plants")
        .RequireAuthorization();
    }
}
