using FluentValidation;
using MediatR;

namespace Nursery.Payment.Api.Features.RetryPayment;

public sealed record RetryPaymentCommand(Guid PaymentId, Guid CustomerId) : IRequest<RetryPaymentResult>;

public sealed record RetryPaymentResult(string ClientSecret);

public sealed class RetryPaymentValidator : AbstractValidator<RetryPaymentCommand>
{
    public RetryPaymentValidator()
    {
        RuleFor(x => x.PaymentId).NotEmpty();
        RuleFor(x => x.CustomerId).NotEmpty();
    }
}