using BuildingBlocks.Common.CQRS;
using MediatR;

namespace Nursery.Payment.Api.Features.HandleStripeWebhook;

public sealed record HandleStripeWebhookCommand(
    string EventId,
    string EventType,
    Guid PaymentId,
    string GatewayPaymentIntentId,
    string? FailureReason) : ICommand<bool>;
