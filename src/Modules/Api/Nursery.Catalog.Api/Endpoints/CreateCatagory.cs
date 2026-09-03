using Carter;
using MediatR;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Api.Endpoints;

public class CreateCatagory : ICarterModule
{
    public record CreateCategoryCommand(string Name, string Description) : IRequest<CreateCatagoryResponse>;
    public record CreateCatagoryResponse(Category Category);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/category", async (CreateCategoryCommand command, ISender sender) =>
        {

            var result = await sender.Send(command);
            return Results.Ok(result);
        });
    }
}

