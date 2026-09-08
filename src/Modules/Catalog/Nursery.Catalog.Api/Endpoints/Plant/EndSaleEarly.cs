using Nursery.Catalog.Application.Plants.Commands.EndSaleEarly;


namespace Nursery.Catalog.Api.Endpoints.Plant;

public class EndSaleEarly : ICarterModule
{
    //public record EndSaleEarlyRequest( Guid PlantId, Guid VariantId, string ModifiedBy);
    public record EndSaleEarlyResponse(bool IsSuccess);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/plants/variants/sale/{plantId:guid}/{variantId:guid}/{modifiedBy}", async (Guid plantId, Guid variantId, string modifiedBy, ISender sender) =>
        {
            var command = new EndSaleEarlyCommand(plantId,variantId, modifiedBy);
            var response = await sender.Send(command);
            return response.Adapt<EndSaleEarlyResponse>();
        }).WithName("End Sale Early")
        .WithSummary("End Sale Early")
        .WithDisplayName("End Sale Early")
        .Produces<EndSaleEarlyResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithTags("Plants");
    }
}
