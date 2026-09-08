using Nursery.Catalog.Application.Catagories.Command.DeleteCategory;
using static Nursery.Catalog.Api.Endpoints.CategoryEP.GetCategory;

namespace Nursery.Catalog.Api.Endpoints.CategoryEP;

public class DeleteCategory : ICarterModule
{
    public record DeleteCategoryRequest(Guid Id);
    public record DeleteCategoryResponse(bool IsDeleted);
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapDelete("/api/v1/category/{Id:guid}", async ([AsParameters] DeleteCategoryRequest request, ISender sender) =>
        {
            var query = request.Adapt<DeleteCategoryCommand>();
            var result = await sender.Send(query);
            return Results.Ok(result.Adapt<DeleteCategoryResponse>());

        }).WithName("Delete Category")
        .Produces<DeleteCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Category")
        .WithDescription("Delete Category")
        .WithTags("Category");
    }
}
