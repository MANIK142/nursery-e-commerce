using MediatR;
using Nursery.Payment.Api.Contracts;
using Nursery.Payment.Api.Enums;

namespace Nursery.Payment.Api.Features.RetryPayment;

public sealed class RetryPaymentHandler(
    IPaymentRepository paymentRepository,
    IPaymentGatewayService gateway)
    : IRequestHandler<RetryPaymentCommand, RetryPaymentResult>
{
    public async Task<RetryPaymentResult> Handle(RetryPaymentCommand request, CancellationToken ct)
    {
        var payment = await paymentRepository.GetByIdAsync(request.PaymentId, ct)
            ?? throw new InvalidOperationException($"Payment {request.PaymentId} not found.");

        if (payment.CustomerId != request.CustomerId)
            throw new UnauthorizedAccessException("Payment does not belong to this customer.");

        if (payment.Status is not PaymentStatus.Failed)
            throw new InvalidOperationException(
                $"Payment can only be retried when Failed. Current status: {payment.Status}.");

        var intent = await gateway.CreatePaymentIntentAsync(
            payment.Amount, payment.Currency, payment.Id, ct);

        payment.StartNewAttempt(intent.Id);

        await paymentRepository.UpdateAsync(payment, ct);

        return new RetryPaymentResult(intent.ClientSecret);
    }
}
