namespace Nursery.Payment.Api.Enums;

public enum AttemptStatus
{
    Initiated,
    RequiresAction,
    Authorized,
    Captured,
    Failed
}
