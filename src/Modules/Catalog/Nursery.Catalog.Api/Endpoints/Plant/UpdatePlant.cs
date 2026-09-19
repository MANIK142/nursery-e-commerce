using BuildingBlocks.Common.CQRS;
using Nursery.Catalog.Application.Plants.Commands.UpdatePlant;

namespace Nursery.Catalog.Api.Endpoints.Plant
{
    public class UpdatePlant : ICarterModule
    {

        public record UpdatePlantRequest(Guid Id, string SkuCode, string Name, string Description, string ModifiedBy, List<Guid> Categories,
                                List<PlantVariantSpec> PlantVariantSpecs, List<ImageSpec> Images);
        public record UpdatePlantResponse(bool IsSuccess);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/api/v1/plants", async (UpdatePlantRequest request, IMediator mediator) =>
            {
                var command = request.Adapt<UpdatePlantCommand>();
                var result = await mediator.Send(command);
                return result.Adapt<UpdatePlantResponse>();
            }).WithName("Update Plant")
        .Produces<UpdatePlantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Plant")
        .WithDescription("Update Plant")
        .WithTags("Plants");
        }
    }
}
