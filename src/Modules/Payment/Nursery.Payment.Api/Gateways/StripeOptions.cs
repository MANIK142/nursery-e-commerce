namespace Nursery.Payment.Api.Gateways;

public sealed class StripeOptions
{
    public const string SectionName = "Stripe";
    public string SecretKey { get; init; } = null!;
    public string WebhookSecret { get; init; } = null!; // used later, in the webhook controller
}
