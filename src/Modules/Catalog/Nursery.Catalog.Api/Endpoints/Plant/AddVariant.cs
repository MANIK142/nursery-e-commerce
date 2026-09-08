using Nursery.Catalog.Application.Plants.Commands.AddVariant;
using static Nursery.Catalog.Api.Endpoints.Plant.CreatePlant;

namespace Nursery.Catalog.Api.Endpoints.Plant;

public class AddVariant : ICarterModule
{
    public record AddVariantRequest(Guid PlantId, string Sku, string VariantName, List<ImageSpec> ImageSpecs, Money RetailPrice, Money WholesalePrice, string ModifiedBy);
    public record AddVariantResponse(Guid Id);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/plants/addvariant", async (AddVariantRequest command, ISender sender) =>
        {
            var RequestCommand = command.Adapt<AddVariantCommand>();
            var result = await sender.Send(RequestCommand);
            var response = result.Adapt<AddVariantResponse>();
            return Results.Ok(response);
        }).WithName("Create Plant Variant")
        .Produces<AddVariantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Plant Variant")
        .WithDescription("Create Plant Variant")
        .WithTags("Plants");
    }
}
