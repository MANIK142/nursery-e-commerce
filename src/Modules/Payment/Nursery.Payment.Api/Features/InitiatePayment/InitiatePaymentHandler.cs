using MediatR;
using Nursery.Payment.Api.Contracts;


namespace Nursery.Payment.Api.Features.InitiatePayment;

public sealed class InitiatePaymentHandler(
    IOrderLookup orderLookup,
    IPaymentGatewayService gateway,
    IPaymentRepository paymentRepository)
    : IRequestHandler<InitiatePaymentCommand, InitiatePaymentResult>
{
    public async Task<InitiatePaymentResult> Handle(InitiatePaymentCommand request, CancellationToken ct)
    {
        var order = await orderLookup.GetOrderPaymentInfoAsync(request.OrderId, ct)
            ?? throw new InvalidOperationException($"Order {request.OrderId} not found.");

        if (order.CustomerId != request.CustomerId)
            throw new UnauthorizedAccessException("Order does not belong to this customer.");

        var payment = new Models.Payment(order.OrderId, order.CustomerId, order.TotalAmount);

        var intent = await gateway.CreatePaymentIntentAsync(
            payment.Amount, payment.Currency, payment.Id, ct);

        payment.StartNewAttempt(intent.Id);

        await paymentRepository.AddAsync(payment, ct);

        return new InitiatePaymentResult(payment.Id, intent.ClientSecret);
    }
}