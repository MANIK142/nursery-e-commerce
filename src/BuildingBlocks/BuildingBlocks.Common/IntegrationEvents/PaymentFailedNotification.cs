

using MediatR;

namespace BuildingBlocks.Common.IntegrationEvents;
public sealed record PaymentFailedNotification(Guid OrderId, Guid PaymentId, string Reason) : INotification;