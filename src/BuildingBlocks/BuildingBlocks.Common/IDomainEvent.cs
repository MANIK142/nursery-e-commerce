using MediatR;


namespace BuildingBlocks.Common;

public interface IDomainEvent :INotification
{
    DateTime OccurredOnUtc { get; }
}
