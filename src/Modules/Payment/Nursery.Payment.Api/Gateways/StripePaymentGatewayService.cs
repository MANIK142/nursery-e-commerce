using Microsoft.Extensions.Options;
using Nursery.Payment.Api.Contracts;
using Nursery.Payment.Api.Exceptions;
using Stripe;

namespace Nursery.Payment.Api.Gateways;

public class StripePaymentGatewayService : IPaymentGatewayService
{
    private readonly PaymentIntentService _paymentIntentService;
    private readonly RefundService _refundService;

    public StripePaymentGatewayService(IOptions<StripeOptions> options)
    {
        StripeConfiguration.ApiKey = options.Value.SecretKey;
        _paymentIntentService = new PaymentIntentService();
        _refundService = new RefundService();
    }

    public async Task<GatewayPaymentIntent> CreatePaymentIntentAsync(
        decimal amount, string currency, Guid paymentId, CancellationToken ct)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = ToSmallestUnit(amount),
                Currency = currency,
                Metadata = new Dictionary<string, string>
                {
                    ["payment_id"] = paymentId.ToString()
                },
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true
                }
            };

            var intent = await _paymentIntentService.CreateAsync(
                options, cancellationToken: ct);

            return new GatewayPaymentIntent(intent.Id, intent.ClientSecret, intent.Status);
        }
        catch (StripeException ex)
        {
            throw new PaymentGatewayException(
                $"Failed to create Stripe PaymentIntent for payment {paymentId}.", ex);
        }
    }

    public async Task<GatewayPaymentIntent> RetrievePaymentIntentAsync(
        string gatewayPaymentIntentId, CancellationToken ct)
    {
        try
        {
            var intent = await _paymentIntentService.GetAsync(
                gatewayPaymentIntentId, cancellationToken: ct);

            return new GatewayPaymentIntent(intent.Id, intent.ClientSecret, intent.Status);
        }
        catch (StripeException ex)
        {
            throw new PaymentGatewayException(
                $"Failed to retrieve Stripe PaymentIntent {gatewayPaymentIntentId}.", ex);
        }
    }

    public async Task RefundAsync(
        string gatewayPaymentIntentId, bool isPartial, decimal? amount, CancellationToken ct)
    {
        try
        {
            var options = new RefundCreateOptions
            {
                PaymentIntent = gatewayPaymentIntentId
            };

            if (isPartial)
            {
                if (amount is null)
                    throw new ArgumentException("Partial refund requires an amount.", nameof(amount));

                options.Amount = ToSmallestUnit(amount.Value);
            }

            await _refundService.CreateAsync(options, cancellationToken: ct);
        }
        catch (StripeException ex)
        {
            throw new PaymentGatewayException(
                $"Failed to refund Stripe PaymentIntent {gatewayPaymentIntentId}.", ex);
        }
    }

    /// <summary>
    /// Stripe expects amounts in the currency's smallest unit.
    /// INR has no fractional subdivision in practice for Stripe (paise),
    /// so this is amount * 100, rounded to avoid floating-point drift.
    /// </summary>
    private static long ToSmallestUnit(decimal amount)
        => (long)Math.Round(amount * 100, MidpointRounding.AwayFromZero);
}
