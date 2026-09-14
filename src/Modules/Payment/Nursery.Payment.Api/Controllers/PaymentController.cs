using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nursery.Payment.Api.Features.HandleStripeWebhook;
using Nursery.Payment.Api.Gateways;
using Stripe;

namespace Nursery.Payment.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Tags("Payments")]
    public sealed class PaymentController(
    IMediator mediator,
    IOptions<StripeOptions> stripeOptions,
    ILogger<PaymentController> logger) : ControllerBase
    {
        private readonly string _webhookSecret = stripeOptions.Value.WebhookSecret;

        [HttpPost("webhook")]
        [AllowAnonymous]
        [EndpointSummary("Handle Stripe Webhook")]
        public async Task<IActionResult> HandleStripeWebhook(CancellationToken ct)
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync(ct);
            var signatureHeader = Request.Headers["Stripe-Signature"];

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, _webhookSecret);
            }
            catch (StripeException ex)
            {
                logger.LogWarning(ex, "Stripe webhook signature verification failed.");
                return BadRequest();
            }

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            if (paymentIntent is null)
            {
                // Not a PaymentIntent-related event (e.g. charge.dispute.created) — ignore.
                return Ok();
            }

            if (!paymentIntent.Metadata.TryGetValue("payment_id", out var paymentIdRaw)
                || !Guid.TryParse(paymentIdRaw, out var paymentId))
            {
                logger.LogWarning(
                    "Stripe webhook {EventId} missing/invalid payment_id metadata on intent {IntentId}.",
                    stripeEvent.Id, paymentIntent.Id);
                return Ok(); // Ack anyway — retrying won't fix bad metadata.
            }

            var command = new HandleStripeWebhookCommand(
                EventId: stripeEvent.Id,
                EventType: stripeEvent.Type,
                PaymentId: paymentId,
                GatewayPaymentIntentId: paymentIntent.Id,
                FailureReason: paymentIntent.LastPaymentError?.Message);

  
            await mediator.Send(command, ct);
       

            return Ok();
        }

        private static bool IsUniqueConstraintViolation() => true; // see note below
    }
}
