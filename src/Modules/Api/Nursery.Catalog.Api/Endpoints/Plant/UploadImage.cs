using Microsoft.AspNetCore.Mvc;
using Nursery.Catalog.Application.Data;

namespace Nursery.Catalog.Api.Endpoints.Plant;

public class UploadImage : ICarterModule
{
    public record UploadImageResponse(string StorageKey, string Url);
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapPost("/api/v1/images/upload", async (HttpRequest request, IBlobStorageService blobStorage, CancellationToken ct) =>
        {
            try
            {
                var form = await request.ReadFormAsync(ct);
                var file = form.Files.GetFile("file");
                if (file is null)
                    return Results.BadRequest("No file provided.");

                var extension = Path.GetExtension(file.FileName);
                var storageKey = $"uploads/{Guid.NewGuid()}{extension}";

                await using var stream = file.OpenReadStream();
                await blobStorage.UploadAsync("nursery", storageKey, stream, file.ContentType, ct);

                var url = blobStorage.GetPublicUrl("nursery", storageKey);
                return Results.Ok(new UploadImageResponse(storageKey, url.ToString()));
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.ToString(), statusCode: 500); // TEMPORARY — full exception + inner exception
            }
        })
        .DisableAntiforgery()
        .WithTags("Images")
        .WithDisplayName("Upload Image")
        .WithName("Upload Image")
        .WithSummary("Upload Image");
    }
    
}
