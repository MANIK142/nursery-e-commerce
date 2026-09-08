using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nursery.Catalog.Application.Dtos;
using Nursery.Catalog.Application.Plants.Commands.CreatePlant;
using Nursery.Catalog.Domain.Models;
using System.Text.Json;

namespace Nursery.Catalog.Api.Endpoints.Plant;

public class CreatePlant : ICarterModule
{
    public record CreatePlantRequest(string Name, string Description, string CreatedBy, List<Guid> Categories, List<PlantVariantSpec> PlantVariantSpecs, List<ImageSpec> Images);
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
         .WithDescription("Create Plant")
         .WithTags("Plants");
    }

   
}
