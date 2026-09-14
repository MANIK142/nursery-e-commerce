


using BuildingBlocks.Common.CQRS;
using FluentValidation;

namespace Nursery.Payment.Api.Features.InitiatePayment;

public sealed record InitiatePaymentCommand(Guid OrderId, Guid CustomerId) : ICommand<InitiatePaymentResult>;

public sealed record InitiatePaymentResult(Guid PaymentId, string ClientSecret);

public sealed class InitiatePaymentValidator : AbstractValidator<InitiatePaymentCommand>
{
    public InitiatePaymentValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}