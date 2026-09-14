using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Nursery.Shippings.Application.Features.CancelShipment;
using Nursery.Shippings.Application.Features.GetShipmentById;
using Nursery.Shippings.Application.Features.GetShipmentsByOrderId;
using Nursery.Shippings.Application.Features.MarkShipmentDelivered;
using Nursery.Shippings.Application.Features.MarkShipmentFailedDelivery;
using Nursery.Shippings.Application.Features.MarkShipmentInTransit;
using Nursery.Shippings.Application.Features.MarkShipmentOutForDelivery;
using Nursery.Shippings.Application.Features.MarkShipmentPacked;
using Nursery.Shippings.Application.Features.MarkShipmentShipped;


namespace Nursery.Shippings.Application.Endpoints;

public sealed class ShipmentEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/shipments").WithTags("Shipments");

        // Customer-facing reads
        group.MapGet("/{shipmentId:guid}", async (Guid shipmentId, ISender sender, CancellationToken ct) =>
        {
            var result = await sender.Send(new GetShipmentByIdQuery(shipmentId), ct);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        }).WithSummary("Get Shipment By Id");

        app.MapGet("/api/v1/orders/{orderId:guid}/shipments",
            async (Guid orderId, ISender sender, CancellationToken ct) =>
            {
                var result = await sender.Send(new GetShipmentsByOrderIdQuery(orderId), ct);
                return Results.Ok(result);
            })
            .WithTags("Shipments")
            .WithSummary("Get Shipments By OrderId");

        // Warehouse/admin transition actions
        group.MapPost("/{shipmentId:guid}/pack", async (Guid shipmentId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new MarkShipmentPackedCommand(shipmentId), ct);
            return Results.NoContent();
        })
        .RequireAuthorization("WarehouseStaff").WithSummary("Update Packed");

        group.MapPost("/{shipmentId:guid}/ship",
            async (Guid shipmentId, MarkShipmentShippedRequest body, ISender sender, CancellationToken ct) =>
            {
                await sender.Send(new MarkShipmentShippedCommand(
                    shipmentId, body.Carrier, body.TrackingNumber, body.TrackingUrl), ct);
                return Results.NoContent();
            })
            .RequireAuthorization("WarehouseStaff").WithSummary("Update Shipped");

        group.MapPost("/{shipmentId:guid}/in-transit", async (Guid shipmentId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new MarkShipmentInTransitCommand(shipmentId), ct);
            return Results.NoContent();
        })
        .RequireAuthorization("WarehouseStaff").WithSummary("Update  In-Transit");

        group.MapPost("/{shipmentId:guid}/out-for-delivery", async (Guid shipmentId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new MarkShipmentOutForDeliveryCommand(shipmentId), ct);
            return Results.NoContent();
        })
        .RequireAuthorization("WarehouseStaff").WithSummary("Update Out for delivery");

        group.MapPost("/{shipmentId:guid}/deliver", async (Guid shipmentId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new MarkShipmentDeliveredCommand(shipmentId), ct);
            return Results.NoContent();
        })
        .RequireAuthorization("WarehouseStaff").WithSummary("Update Delivered"); 

        group.MapPost("/{shipmentId:guid}/fail-delivery", async (Guid shipmentId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new MarkShipmentFailedDeliveryCommand(shipmentId), ct);
            return Results.NoContent();
        })
        .RequireAuthorization("WarehouseStaff").WithSummary("Update Delivery Failed"); ;

        group.MapPost("/{shipmentId:guid}/cancel", async (Guid shipmentId, ISender sender, CancellationToken ct) =>
        {
            await sender.Send(new CancelShipmentCommand(shipmentId), ct);
            return Results.NoContent();
        })
        .RequireAuthorization("WarehouseStaff").WithSummary("Update Cancel"); 
    }
}

public sealed record MarkShipmentShippedRequest(string Carrier, string TrackingNumber, string? TrackingUrl);