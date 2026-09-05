using Nursery.Catalog.Application.Plants.Commands.RemoveVariant;

namespace Nursery.Catalog.Api.Endpoints.Plant;

public class RemoveVariant : ICarterModule
{
    public record RemoveVariantRequest(Guid VariantId, Guid PlantId, string ModifiedBy);
    public record RemoveVariantResponse(bool IsSuccess);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/plants/variants/{plantId:guid}/{variantId:guid}/{modifiedBy}", async (Guid plantId, Guid variantId, string modifiedBy, ISender sender) =>
        {
            var command = new RemoveVariantCommand( variantId, plantId, modifiedBy);
            var response = await sender.Send(command);
            return response.Adapt<RemoveVariantResponse>();
        }).WithName("Remove Variant")
        .WithSummary("Remove Variant")
        .WithDisplayName("Remove Variant")
        .Produces<RemoveVariantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithTags("Plants");
    }
}
