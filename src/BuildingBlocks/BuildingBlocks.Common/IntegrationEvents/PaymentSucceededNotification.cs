using MediatR;


namespace BuildingBlocks.Common.IntegrationEvents;

public sealed record PaymentSucceededNotification(Guid OrderId, Guid PaymentId) : INotification;