namespace Nursery.Payment.Api.Enums;

public enum PaymentStatus
{
    Pending,
    Processing,
    Succeeded,
    Failed,
    Cancelled, 
    Refunded,
    PartiallyRefunded
}