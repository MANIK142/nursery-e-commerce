using BuildingBlocks.Common.CQRS;
using BuildingBlocks.Common.IntegrationEvents;
using MediatR;
using Nursery.Payment.Api.Contracts;
using Nursery.Payment.Api.Enums;
using Nursery.Payment.Api.Models;
using Nursery.Payment.Api.Persistance.Repository;
using Stripe.Climate;
using Stripe.V2;
using System.Text.Json;

namespace Nursery.Payment.Api.Features.HandleStripeWebhook;

public class HandleStripeWebhookHandler(
    IPaymentRepository paymentRepository,
    IProcessedWebhookEventRepository processedEvents, IPublisher publisher) : ICommandHandler<HandleStripeWebhookCommand, bool>
{
    public async Task<bool> Handle(HandleStripeWebhookCommand request, CancellationToken ct)
    {
        if (await processedEvents.ExistsAsync(request.EventId, ct))
            return false;

        var payment = await paymentRepository.GetByIdAsync(request.PaymentId, ct)
            ?? throw new InvalidOperationException($"Payment {request.PaymentId} not found for webhook event {request.EventId}.");

        var attempt = payment.Attempts
            .FirstOrDefault(a => a.GatewayPaymentIntentId == request.GatewayPaymentIntentId)
            ?? throw new InvalidOperationException(
                $"No attempt with intent {request.GatewayPaymentIntentId} on payment {request.PaymentId}.");

        switch (request.EventType)
        {
            case "payment_intent.requires_action":
                payment.MarkAttemptRequiresAction(attempt.Id);
                break;

            case "payment_intent.processing":
                payment.MarkAttemptAuthorized(attempt.Id);
                break;

            case "payment_intent.succeeded":
                payment.MarkSucceeded(attempt.Id);
                break;

            case "payment_intent.payment_failed":
                payment.MarkAttemptFailed(attempt.Id, request.FailureReason ?? "Unknown gateway failure");
                break;

            default:
                // Unhandled event types are ignored, not errors — Stripe sends
                // many event types we don't act on (e.g. charge.updated).
                return false;
        }

        await paymentRepository.UpdateAsync(payment, ct);
        await processedEvents.MarkProcessedAsync(request.EventId, ct);

        if (payment.Status is PaymentStatus.Succeeded)
        {

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = "payment.success",
                Content = JsonSerializer.Serialize(new PaymentSucceededNotification(payment.OrderId, payment.Id)),
                OccurredOnUtc = DateTime.UtcNow
            };

            await paymentRepository.AddOutboxMessageAsync(outboxMessage, ct);

            //await publisher.Publish(new PaymentSucceededNotification(payment.OrderId, payment.Id), ct);
        }
        else if (payment.Status is PaymentStatus.Failed)
        {

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = "payment.failed",
                Content = JsonSerializer.Serialize(new PaymentFailedNotification(payment.OrderId, payment.Id, request.FailureReason ?? "Payment failed")),
                OccurredOnUtc = DateTime.UtcNow
            };

            await paymentRepository.AddOutboxMessageAsync(outboxMessage, ct);

            //await publisher.Publish(
            //    new PaymentFailedNotification(payment.OrderId, payment.Id, request.FailureReason ?? "Payment failed"), ct);
        }

        return true;
    }
}
