
using BuildingBlocks.Common.Caching;
using MediatR;

namespace BuildingBlocks.Common.Behaviors;
public class CacheInvalidationBehavior<TRequest, TResponse>(ICacheService cache)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheInvalidatorCommand, IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next(); // only invalidate on success — a failed/validation-rejected command shouldn't evict anything
        await cache.RemoveByPrefixAsync(request.CacheKeyPrefix, ct);
        return response;
    }
}