using BuildingBlocks.Common;
using BuildingBlocks.Common.IntegrationEvents;
using MediatR;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Nursery.Payment.Api.Enums;
using Nursery.ShareKernel;

namespace Nursery.Payment.Api.Models;
public class Payment :BaseDomainModel
{
    public Guid Id { get; private set;}
    public Guid OrderId { get; private set;}
    public Guid CustomerId { get; private set;}
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = "INR";
    public PaymentStatus Status { get; private set;}

    private readonly List<PaymentAttempt> _attempts = new();
    public IReadOnlyCollection<PaymentAttempt> Attempts => _attempts.AsReadOnly();

    private Payment() { } // EF Core

    public Payment(Guid orderId, Guid customerId, decimal amount)
    {
        Id = Guid.NewGuid();
        OrderId = Guard.AgainstDefault(orderId, nameof(orderId));
        CustomerId = Guard.AgainstDefault(customerId, nameof(customerId));
        Amount = Guard.AgainstNegative(amount, nameof(amount));
        Currency = "inr";
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public PaymentAttempt StartNewAttempt(string gatewayPaymentIntentId)
    {
        if (Status is PaymentStatus.Succeeded)
            throw new InvalidOperationException("Payment has already succeeded; cannot start a new attempt.");

        if (Status is PaymentStatus.Cancelled)
            throw new InvalidOperationException("Payment is cancelled; cannot start a new attempt.");

        var inFlight = _attempts.Any(a =>
            a.AttemptStatus is AttemptStatus.Initiated
                or AttemptStatus.RequiresAction
                or AttemptStatus.Authorized);

        if (inFlight)
            throw new InvalidOperationException("An attempt is already in-flight for this payment.");

        var attempt = new PaymentAttempt(Id, gatewayPaymentIntentId, _attempts.Count + 1);
        _attempts.Add(attempt);

        Status = PaymentStatus.Processing;
        ModifiedAt = DateTime.UtcNow;

        return attempt;
    }

    public void MarkAttemptRequiresAction(Guid attemptId)
        => GetAttempt(attemptId).MarkRequiresAction();

    public void MarkAttemptAuthorized(Guid attemptId)
        => GetAttempt(attemptId).MarkAuthorized();

    public async void MarkSucceeded(Guid attemptId)
    {
        GetAttempt(attemptId).MarkCaptured();
        Status = PaymentStatus.Succeeded;
        ModifiedAt = DateTime.UtcNow;

    }

    public void MarkAttemptFailed(Guid attemptId, string reason)
    {
        GetAttempt(attemptId).MarkFailed(reason);

        // Payment itself only moves to Failed if every attempt has failed.
        if (_attempts.All(a => a.AttemptStatus is AttemptStatus.Failed))
        {
            Status = PaymentStatus.Failed;
            ModifiedAt = DateTime.UtcNow;
        }
    }

    public void Cancel()
    {
        if (Status is PaymentStatus.Succeeded)
            throw new InvalidOperationException("Cannot cancel a succeeded payment.");

        Status = PaymentStatus.Cancelled;
        ModifiedAt = DateTime.UtcNow;
    }

    public void Refund(bool isPartial)
    {
        if (Status is not PaymentStatus.Succeeded and not PaymentStatus.PartiallyRefunded)
            throw new InvalidOperationException("Only a succeeded payment can be refunded.");

        Status = isPartial ? PaymentStatus.PartiallyRefunded : PaymentStatus.Refunded;
        ModifiedAt = DateTime.UtcNow;
    }

    private PaymentAttempt GetAttempt(Guid attemptId)
        => _attempts.FirstOrDefault(a => a.Id == attemptId)
           ?? throw new InvalidOperationException($"Attempt {attemptId} not found on this payment.");

}
