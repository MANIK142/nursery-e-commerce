using Nursery.Catalog.Application.Plants.Commands.DeletePlant;

namespace Nursery.Catalog.Api.Endpoints.Plant
{
    public class DeletePlant : ICarterModule
    {
        public record DeletePlantRequest(Guid Id);
        public record DeletePlantResponse(bool IsSuccess);
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/api/v1/plants/{Id:guid}", async ([AsParameters] DeletePlantRequest request, ISender sender) =>
            {
                var command = request.Adapt<DeletePlantCommand>();
                var result = await sender.Send(command);
                return result.Adapt<DeletePlantResponse>();
            }).WithName("Delete Plant")
            .Produces<DeletePlantResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Plant")
            .WithDescription("Delete Plant");
        }
    }
}
