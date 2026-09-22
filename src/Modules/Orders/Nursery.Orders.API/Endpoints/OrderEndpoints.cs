using BuildingBlocks.Common.CQRS;
using Carter;
using Mapster;
using MediatR;
using Nursery.Orders.API.Extenstions;
using Nursery.Orders.Application.Dto;
using Nursery.Orders.Application.Features.CancelOrder;
using Nursery.Orders.Application.Features.CreateOrder;
using Nursery.Orders.Application.Features.GetOrder;
using Nursery.Orders.Application.Features.GetOrderById;
using Nursery.Orders.Domain.ValueObjects;
using System.Security.Claims;
using BuildingBlocks.Common;
using Nursery.Orders.Application.Features.GetAllOrder;
namespace Nursery.Orders.API.Endpoints;

public class OrderEndpoints : ICarterModule
{
    public record CreateOrderRequest(Guid CustomerId, AddressDto BillingAddress, AddressDto ShippingAddress, List<CreateOrderItemDto> Items);
    public record CreateOrderResponse(Guid OrderId);

    public record GetOrdersRequest(Guid CustomerId);
    public record GetOrdersResponse(IEnumerable<OrderDto> Orders);

    public record CancelOrderRequest(Guid OrderId, Guid CustomerId);

    public record CancelOrderResponse(bool IsSuccess);

    public record GetOrdersByIdResponse(OrderDto Order);

    public record GetAllOrderRequest(int? PageNumber, int? PageSize, Guid? Id, string? FilterBy, string? FilterValue) ;

    public record GetAllOrderResponse(IEnumerable<OrderDto> Orders);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group =  app.MapGroup("/api/v1/orders").WithTags("Orders");

        group.MapPost("/", async (CreateOrderRequest request, ISender sender, ClaimsPrincipal user) =>
        {
            var command = request.Adapt<CreateOrderCommand>();
            var result = await sender.Send(command);
            return result.Adapt<CreateOrderResponse>();
        }).WithDescription("Create Order")
        .WithSummary("Create Order")
        .Produces<CreateOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapGet("/", async ( ISender sender, ClaimsPrincipal user) =>
        {
            var customerId = user.GetCustomerId(); // or Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!)
            var query = new GetOrderQuery(customerId);
            var command = query.Adapt<GetOrderQuery>();
            var result = await sender.Send(command);
            return result.Adapt<GetOrdersResponse>();
        }).WithDescription("Get Orders By Customer Id")
      .WithSummary("Get Orders By Customer Id")
      .Produces<GetOrdersResponse>(StatusCodes.Status200OK)
      .ProducesProblem(StatusCodes.Status400BadRequest);


        // GET /api/v1/orders/{orderId} — get a single order, scoped to the caller
        group.MapGet("/{orderId:guid}", async (Guid orderId, ISender sender, ClaimsPrincipal user) =>
        {
            var customerId = user.GetCustomerId();
            var query = new GetOrderByIdQuery(orderId, customerId);

            var result = await sender.Send(query);
            if (result is null)
                return Results.NotFound();

            return Results.Ok(result.Adapt<GetOrdersByIdResponse>());
        })
        .WithName("GetOrderById")
        .WithSummary("Get Order By Id")
        .WithDescription("Returns a single order. Scoped to the authenticated customer — returns 404 if the order belongs to someone else.")
        .Produces<OrderDto>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);


        // GET /api/v1/orders/{orderId} — get a single order, scoped to the caller
        group.MapGet("/getallorders", async ([AsParameters] GetAllOrderRequest request, ISender sender, ClaimsPrincipal user) =>
        {
            var customerId = user.GetCustomerId();
            var query = request.Adapt<GetAllOrderQuery>();

            var result = await sender.Send(query);
            if (result is null)
                return Results.NotFound();

            return Results.Ok(result.Adapt<GetAllOrderResponse>());
        })
        .WithName("GetAllOrder")
        .WithSummary("Get All Orders")
        .WithDescription("Returns a single order. Scoped to the authenticated customer — returns 404 if the order belongs to someone else.")
        .Produces<GetAllOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound);



        group.MapPut("/{orderId:guid}/cancel", async (Guid orderId, ISender sender, ClaimsPrincipal user) =>
        {
            var customerId = user.GetCustomerId();
            var command = new CancelOrderCommand(orderId, customerId);

            var result = await sender.Send(command);
            return Results.Ok(result.Adapt<CancelOrderResponse>());
        })
        .WithName("CancelOrder")
        .WithSummary("Cancel Order")
        .WithDescription("Cancels an order that has not yet been paid or shipped. Fails if the order is in a non-cancellable status.")
        .Produces<CancelOrderResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest) // e.g. already paid/shipped
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
