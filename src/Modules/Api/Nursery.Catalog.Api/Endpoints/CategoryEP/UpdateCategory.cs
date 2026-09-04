using Nursery.Catalog.Application.Catagories.Command.UpdateCategory;
using static Nursery.Catalog.Api.Endpoints.Plant.UpdatePlant;

namespace Nursery.Catalog.Api.Endpoints.CategoryEP;

public class UpdateCategory : ICarterModule
{
    public record UpdateCategoryRequest(Guid Id, string Name, string Description, string UpdatedBy) : IRequest<UpdateCategoryResponse>;
    public record UpdateCategoryResponse(bool IsSuccess);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/v1/category/update", async (UpdateCategoryRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateCategoryCommand>();
            var response = await sender.Send(command);
            var result = response.Adapt<UpdateCategoryResponse>();
            return Results.Ok(result);
        }).WithName("Update Category")
        .Produces<UpdateCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Update Category")
        .WithDescription("Update Category");
    }
}


