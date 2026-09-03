
using MediatR;
namespace BuildingBlocks.Common.CQRS;

public interface ICommandHandler<in TCommand, TResponse> 
    : IRequestHandler<TCommand, TResponse> 
    where TCommand : ICommand<TResponse>
{
}
