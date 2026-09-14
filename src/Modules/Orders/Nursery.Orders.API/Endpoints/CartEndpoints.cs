
using Amazon.S3.Model;
using Carter;
using MediatR;
using Nursery.Orders.API.Extenstions;
using Nursery.Orders.Application.Features.AddItemToCart;
using Nursery.Orders.Application.Features.DecreaseCartItemQuantity;
using Nursery.Orders.Application.Features.GetCarts;
using Nursery.Orders.Application.Features.IncreaseCartItemQuantity;
using Nursery.Orders.Application.Features.RemoveItemFromCart;
using System.Security.Claims;
using BuildingBlocks.Common;

namespace Nursery.Orders.API.Endpoints;

public class CartEndpoints : ICarterModule
{
    public record AddItemToCartRequest(Guid PlantvariantId);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/carts").WithTags("Carts");


        //group.MapGet("/debug/claims", (ClaimsPrincipal user) =>
        //{
        //    return Results.Ok(user.Claims.Select(c => new { c.Type, c.Value }));
        //}).RequireAuthorization();

        group.MapGet("/", async (ISender mediator, ClaimsPrincipal user) =>
        {
            var customerId = user.GetCustomerId();
            var command = new GetCartQuery(customerId);
            var cart = await mediator.Send(command);
            return cart is null ? Results.NoContent() : Results.Ok(cart);
        }).WithName("Get Cart")
        .Produces<GetCartResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Cart")
        .WithDescription("Get Cart");


        
        group.MapPost("/items", async (AddItemToCartRequest request,ISender mediator,ClaimsPrincipal user) =>
        {
            var customerId = user.GetCustomerId();
            var result = await mediator.Send(new AddItemToCartCommand(customerId, request.PlantvariantId));
            return Results.Ok(result);
        }).WithName("Add Items to Cart")
        .Produces<AddItemToCartResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Add Items to Cart")
        .WithDescription("Add Items to Cart");



        group.MapDelete("/items/{plantVariantId:guid}", async (
            Guid plantVariantId,
            ISender mediator,
            ClaimsPrincipal user) =>
        {
            var customerId = user.GetCustomerId();
            var result = await mediator.Send(new RemoveItemFromCartCommand(customerId, plantVariantId));
            return Results.Ok(result);
        }).WithName("Delete Items to Cart")
        .Produces<RemoveItemFromCartResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Delete Items to Cart")
        .WithDescription("Delete Items to Cart");

        group.MapPost("/items/{plantVariantId:guid}/increase", async (
            Guid plantVariantId,
            ISender mediator,
            ClaimsPrincipal user) =>
        {
            var customerId = user.GetCustomerId();
            var result = await mediator.Send(new IncreaseCartItemQuantityCommand(customerId, plantVariantId));
            return Results.Ok(result);
        }).WithName("Increase Items Quantity to Cart")
        .Produces<IncreaseCartItemQuantityResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Increase Items Quantity  to Cart")
        .WithDescription("Increase Items Quantity  to Cart");

        group.MapPost("/items/{plantVariantId:guid}/decrease", async (
            Guid plantVariantId,
            ISender mediator,
            ClaimsPrincipal user) =>
        {
            var customerId = user.GetCustomerId();
            var result = await mediator.Send(new DecreaseCartItemQuantityCommand(customerId, plantVariantId));
            return Results.Ok(result);
        }).WithName("Decrease Items Quantity to Cart")
        .Produces<DecreaseCartItemQuantityResult>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Decrease Items Quantity  to Cart")
        .WithDescription("Decrease Items Quantity  to Cart");
    }
}
