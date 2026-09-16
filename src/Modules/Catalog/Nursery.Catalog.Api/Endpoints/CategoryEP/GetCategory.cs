

using Nursery.Catalog.Application.Catagories.Queries.GetCategory;
using Nursery.Catalog.Application.Dtos;
using Nursery.Catalog.Domain.Models;
namespace Nursery.Catalog.Api.Endpoints.CategoryEP;
public class GetCategory : ICarterModule
{
    public record GetCategoryRequest(int? PageNumber, int? PageSize,Guid? Id,string? FilterBy,string? FilterByValue);

    public record GetCategoryResponse(IEnumerable<CategoryDto> Categories);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/category", async ([AsParameters] GetCategoryRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetCategoryQuery>();
            var result = await sender.Send(query);
            return Results.Ok(result.Adapt<GetCategoryResponse>());

        }).WithName("Get Category")
        .Produces<GetCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Category")
        .WithDescription("Get Category")
        .WithTags("Category");
    }
}
