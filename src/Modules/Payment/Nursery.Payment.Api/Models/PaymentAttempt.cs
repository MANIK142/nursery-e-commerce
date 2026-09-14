using BuildingBlocks.Common;
using Nursery.Payment.Api.Enums;
using Nursery.ShareKernel;
using System.Net.NetworkInformation;

namespace Nursery.Payment.Api.Models;

public class PaymentAttempt :BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid PaymentId { get; private set; }
    public string GatewayProvider { get; private set; } = "Stripe";
    public string GatewayPaymentIntentId { get; private set; } = default!;
    public int AttemptNumber { get; private set; }
    public AttemptStatus AttemptStatus { get; private set; } = default!;
    public string? FailureReason { get; private set; }

    internal PaymentAttempt(Guid paymentId,string gatewayPaymentIntentId,int attemptNumber)
    {
        Id = Guid.NewGuid();
        PaymentId = paymentId;
        GatewayPaymentIntentId = Guard.AgainstNullOrWhiteSpace(gatewayPaymentIntentId, nameof(gatewayPaymentIntentId));
        AttemptNumber = attemptNumber;
        AttemptStatus = AttemptStatus.Initiated;
        CreatedAt = DateTime.UtcNow;
    }

    internal void MarkRequiresAction()
    {
        //Guard.Against.OutOfRange(
        //    AttemptStatus, nameof(AttemptStatus), AttemptStatus.Initiated, AttemptStatus.Initiated,
        //    "Attempt must be Initiated to require action.");
        AttemptStatus = AttemptStatus.RequiresAction;
        SetModified("");
    }

    internal void MarkAuthorized()
    {
        AttemptStatus = AttemptStatus.Authorized;
        SetModified("");
    }

    internal void MarkCaptured()
    {
        AttemptStatus = AttemptStatus.Captured;
        SetModified("");
    }

    internal void MarkFailed(string reason)
    {
        AttemptStatus = AttemptStatus.Failed;
        FailureReason = Guard.AgainstNullOrWhiteSpace(reason, nameof(reason));
        SetModified("");
    }
}
