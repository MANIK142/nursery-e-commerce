using BuildingBlocks.Common;
using Carter;
using MediatR;
using Microsoft.AspNetCore;
using Nursery.Payment.Api.Features.HandleStripeWebhook;
using Nursery.Payment.Api.Features.InitiatePayment;
using Nursery.Payment.Api.Features.RetryPayment;
using Stripe;
using System.Security.Claims;

namespace Nursery.Payment.Api.Endpoints;

public class PaymentEndpoints : ICarterModule
{
    private static readonly HashSet<string> HandledEventTypes =
      [
          "payment_intent.requires_action",
            "payment_intent.processing",
            "payment_intent.succeeded",
            "payment_intent.payment_failed"
      ];
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1/payments")
            .WithTags("Payments");

        group.MapPost("initiate", InitiatePayment).WithSummary("Initiate Payment").Produces<IResult>().RequireAuthorization();
        group.MapPost("{paymentId:guid}/retry", RetryPayment).WithSummary("Retry Payment").Produces<IResult>().RequireAuthorization();

        group.MapPost("webhook", HandleStripeWebhook)
          .WithSummary("Stripe webhook receiver")
          .AllowAnonymous()
          .ExcludeFromDescription();

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

    private static async Task<IResult> HandleStripeWebhook(
        HttpRequest httpRequest,
        IConfiguration configuration,
        IMediator mediator,
        ILogger<PaymentEndpoints> logger,
        CancellationToken ct)
    {
        // 1. Raw body, untouched. Signature verification is computed over the exact bytes Stripe sent,
        //    so never bind this to a DTO or re-serialise it before verifying.
        using var reader = new StreamReader(httpRequest.Body);
        var json = await reader.ReadToEndAsync(ct);

        var signature = httpRequest.Headers["Stripe-Signature"].ToString();
        var webhookSecret = configuration["Stripe:WebhookSecret"]
            ?? throw new InvalidOperationException("Stripe:WebhookSecret is not configured.");

        // 2. Verify the signature. Failure => 400 (not retried usefully, and not from Stripe).
        Event stripeEvent;
        try
        {
            stripeEvent = EventUtility.ConstructEvent(
                json, signature, webhookSecret,
                throwOnApiVersionMismatch: false); // don't reject events because the account's API version differs from the SDK's
        }
        catch (StripeException ex)
        {
            logger.LogWarning(ex, "Rejected Stripe webhook: invalid signature or payload.");
            return Results.BadRequest();
        }

        // 3. Ignore event types we don't act on. 200 so Stripe doesn't retry them.
        if (!HandledEventTypes.Contains(stripeEvent.Type) || stripeEvent.Data.Object is not PaymentIntent intent)
            return Results.Ok();

        // 4. Correlate back to our Payment via the metadata set in InitiatePayment.
        //    Signed but unusable events (e.g. `stripe trigger ...` test events) get 200, not an error,
        //    otherwise Stripe retries them for days.
        if (!intent.Metadata.TryGetValue("payment_id", out var rawPaymentId) || !Guid.TryParse(rawPaymentId, out var paymentId))
        {
            logger.LogWarning("Stripe event {EventId} ({EventType}) has no valid payment_id metadata; ignoring.",
                stripeEvent.Id, stripeEvent.Type);
            return Results.Ok();
        }

        // 5. Dispatch. Exceptions (e.g. payment not found) bubble up as 500, so Stripe retries with backoff.
        var command = new HandleStripeWebhookCommand(
            EventId: stripeEvent.Id,
            EventType: stripeEvent.Type,
            PaymentId: paymentId,
            GatewayPaymentIntentId: intent.Id,
            FailureReason: intent.LastPaymentError?.Message);

        await mediator.Send(command, ct);

        return Results.Ok();
    }
}

public sealed record InitiatePaymentRequest(Guid OrderId);