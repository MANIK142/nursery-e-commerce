using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using BuildingBlocks.Utility;
using Carter.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
using Nursery.Catalog.Application.Data;
using static Nursery.Catalog.Api.Endpoints.Images.UploadImage;

namespace Nursery.Catalog.Api.Endpoints.Images;

public class DeleteImage : ICarterModule
{
    public record DeleteResponse(bool IsSuccess);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/images/{storagekey}", async (string storageKey, IBlobStorageService blobStorage, ISender sender, CancellationToken ct) =>
        {
            try
            {
                await blobStorage.DeleteAsync(AppConstants.ContainerName, storageKey, ct);
                return Results.Ok(new DeleteResponse(true));
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.ToString(), statusCode: 500);
            }
        }).DisableAntiforgery()
          .WithTags("Images")
          .WithSummary("Delete Image")
          .Produces(StatusCodes.Status200OK)
          .ProducesProblem(StatusCodes.Status500InternalServerError);
          
    }
}
