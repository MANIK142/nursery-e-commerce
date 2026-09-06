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
    public record CreatePlantRequest(string Name, string Description, string CreatedBy, List<Guid> Categories, List<PlantVariantSpec> PlantVariantSpecs, List<ImageUploadDto> Images);
    public record CreatePlantResponse(Guid Id);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/plants", async ([FromForm]  CreatePlantRequest command, ISender sender) =>
         {
             var RequestCommand = command.Adapt<CreatePlantCommand>();
             var result = await sender.Send(RequestCommand);
             var response = result.Adapt<CreatePlantResponse>();
             return Results.Ok(response);
         }).WithName("Create Plant")
         .Accepts<CreatePlantRequest>("multipart/form-data")
         .Produces<CreatePlantResponse>(StatusCodes.Status200OK)
         .ProducesProblem(StatusCodes.Status400BadRequest)
         .WithSummary("Create Plant")
         .WithDescription("Create Plant")
         .WithTags("Plants");
    }

    //public record CreatePlantMetaDto(
    //    string Name,
    //    string Description,
    //    string CreatedBy,
    //    List<Guid> Categories,
    //    List<PlantVariantSpec> PlantVariantSpecs,
    //    List<ImageMetaDto> ImageMeta);

    //public record ImageMetaDto(string AltText, int SortOrder);

    //public record CreatePlantResponse(Guid Id);

    //public void AddRoutes(IEndpointRouteBuilder app)
    //{
    //    app.MapPost("/api/v1/plants", async (
    //        [FromForm] string meta,
    //        [FromForm] List<IFormFile> images,
    //        ISender sender) =>
    //    {
    //        var dto = JsonSerializer.Deserialize<CreatePlantMetaDto>(meta, new JsonSerializerOptions
    //        {
    //            PropertyNameCaseInsensitive = true
    //        })!;

    //        if (images.Count != dto.ImageMeta.Count)
    //            return Results.BadRequest("Number of image files must match number of image metadata entries.");

    //        var imageDtos = images.Zip(dto.ImageMeta, (file, meta) =>
    //            new ImageUploadDto(Guid.NewGuid(), Guid.NewGuid(), file, meta.AltText, true)).ToList();

    //        var command = dto.Adapt<CreatePlantCommand>();
    //        command = command with { Images = imageDtos };

    //        var result = await sender.Send(command);
    //        var response = result.Adapt<CreatePlantResponse>();
    //        return Results.Ok(response);
    //    })
    //    .WithName("Create Plant")
    //    .Accepts<CreatePlantMetaDto>("multipart/form-data")
    //    .DisableAntiforgery()
    //    .Produces<CreatePlantResponse>(StatusCodes.Status200OK)
    //    .ProducesProblem(StatusCodes.Status400BadRequest)
    //    .WithSummary("Create Plant")
    //    .WithDescription("Create Plant")
    //    .WithTags("Plants");
    //}
}
