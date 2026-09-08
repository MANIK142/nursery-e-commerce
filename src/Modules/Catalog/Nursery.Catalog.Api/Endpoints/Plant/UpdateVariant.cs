using BuildingBlocks.Common.CQRS;
using Nursery.Catalog.Application.Plants.Commands.UpdateVariant;

namespace Nursery.Catalog.Api.Endpoints.Plant;

public class UpdateVariant : ICarterModule
{
    public record UpdateVariantRequest(Guid Id, Guid PlantId, string Sku, string VariantName, Money RetailPrice, Money WholesalePrice, string ModifiedBy);
    public record UpdateVariantResponse(bool IsSuccess);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/plants/updatevariant", async (UpdateVariantRequest Request, ISender sender) =>
        {
            var command = Request.Adapt<UpdateVariantCommand>();
            var response = await sender.Send(command);
            return response.Adapt<UpdateVariantResponse>();
        }).WithName("Update Variant")
        .WithDescription("Update Variant")
        .WithSummary("Update Variant")
        .Produces<UpdateVariantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithTags("Plants"); 
    }
}
