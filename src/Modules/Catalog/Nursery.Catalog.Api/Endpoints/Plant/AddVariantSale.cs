using BuildingBlocks.Common.CQRS;
using Nursery.Catalog.Application.Plants.Commands.AddSale;

namespace Nursery.Catalog.Api.Endpoints.Plant;

public class AddVariantSale : ICarterModule
{
    public record AddSaleRequest(Guid PlantId, Guid PlantVariantId, Money SalePrice, DateTime StartsAtUtc, DateTime EndsAtUtc, string ModifiedBy) : ICommand<AddSaleResult>;
    public record AddSaleResponse(bool IsSuccess);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/plants/addvariantsale", async (AddSaleRequest Request, ISender sender) =>
        {
            var command = Request.Adapt<AddSaleCommand>();
            var result = await sender.Send(command);
            return result.Adapt<AddSaleResult>();

        }).WithName("Add Sale Price")
        .WithDisplayName("Add Sale Price")
        .WithSummary("Add Sale Price")
        .Produces<AddSaleResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithTags("Plants");
    }
}
