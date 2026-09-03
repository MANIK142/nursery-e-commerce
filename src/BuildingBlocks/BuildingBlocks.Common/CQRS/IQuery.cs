
using MediatR;
namespace BuildingBlocks.Common.CQRS;
public interface IQuery : IQuery<Unit>;
public interface IQuery<out TResponse> : IRequest<TResponse>
{
}


