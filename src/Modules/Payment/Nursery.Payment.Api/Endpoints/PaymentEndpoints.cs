using Carter;
using MediatR;
using BuildingBlocks.Common;
using Nursery.Payment.Api.Features.InitiatePayment;
using Nursery.Payment.Api.Features.RetryPayment;
using System.Security.Claims;

namespace Nursery.Payment.Api.Endpoints;

public class PaymentEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/payments")
            .WithTags("Payments");

        group.MapPost("initiate", InitiatePayment).WithSummary("Initiate Payment").Produces<IResult>().RequireAuthorization();
        group.MapPost("{paymentId:guid}/retry", RetryPayment).WithSummary("Retry Payment").Produces<IResult>().RequireAuthorization();
    }

    private static async Task<IResult> InitiatePayment(
        InitiatePaymentRequest request,
        ClaimsPrincipal user,
        IMediator mediator,
        CancellationToken ct)
    {
        var customerId = user.GetCustomerId(); // extension over your JWT customer_id claim

        var command = new InitiatePaymentCommand(request.OrderId, customerId);
        var result = await mediator.Send(command, ct);

        return Results.Ok(result);
    }

    private static async Task<IResult> RetryPayment(
        Guid paymentId,
        ClaimsPrincipal user,
        IMediator mediator,
        CancellationToken ct)
    {
        var customerId = user.GetCustomerId();

        var command = new RetryPaymentCommand(paymentId, customerId);
        var result = await mediator.Send(command, ct);

        return Results.Ok(result);
    }
}

public sealed record InitiatePaymentRequest(Guid OrderId);