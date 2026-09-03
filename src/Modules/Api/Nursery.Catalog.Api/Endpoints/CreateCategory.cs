using Carter;
using MediatR;
using Nursery.Catalog.Application.Catagories.Command.CreateCatagory;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Api.Endpoints;

public class CreateCategory : ICarterModule
{
    public record CreateCategoryRequest(string Name, string Description,string CreatedBy) : IRequest<CreateCategoryResponse>;
    public record CreateCategoryResponse(Guid Id);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/category", async (CreateCategoryRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateCategoryCommand>();
            var response = await sender.Send(command);
            var result = response.Adapt<CreateCategoryResponse>();
            return Results.Ok(result);
        });
    }
}

