namespace Nursery.Payment.Api.Contracts;
public interface IPaymentGatewayService
{
    Task<GatewayPaymentIntent> CreatePaymentIntentAsync(decimal amount, string currency, Guid paymentId, CancellationToken ct);
    Task<GatewayPaymentIntent> RetrievePaymentIntentAsync(string gatewayPaymentIntentId, CancellationToken ct);
    Task RefundAsync(string gatewayPaymentIntentId, bool isPartial, decimal? amount, CancellationToken ct);
}

public sealed record GatewayPaymentIntent(string Id, string ClientSecret, string Status);