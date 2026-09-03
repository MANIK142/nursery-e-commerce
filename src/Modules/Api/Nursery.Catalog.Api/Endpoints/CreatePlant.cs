using Carter;
using Mapster;
using MediatR;
using Nursery.Catalog.Application.Plants.Commands.CreatePlant;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Api.Endpoints;

public class CreatePlant : ICarterModule
{
    public record CreatePlantRequest(string SkuCode, string Name, string Description, decimal RetailPrice, string ImageUrl, string CreatedBy, List<Guid> Categories);
    public record CreatePlantResponse(Guid Id);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
       app.MapPost("/api/v1/plants", async (CreatePlantRequest command, ISender sender) =>
        {
            var RequestCommand = command.Adapt<CreatePlantCommand>();
            var result = await sender.Send(RequestCommand);
            var response = result.Adapt<CreatePlantResponse>();
            return Results.Ok(response);
        }).WithName("Create Plant")
        .Produces<CreatePlantResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create Plant")
        .WithDescription("Create Plant");
    }
}
