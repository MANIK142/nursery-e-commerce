
using BuildingBlocks.Common.Caching;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Common.Behaviors;

public class CachingBehavior<TRequest, TResponse>(
    ICacheService cache,
    ILogger<CachingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheableQuery, IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var cached = await cache.GetAsync<TResponse>(request.CacheKey, ct);
        if (cached is not null)
        {
            logger.LogInformation("Cache hit: {CacheKey}", request.CacheKey);
            return cached;
        }

        var response = await next();
        await cache.SetAsync(request.CacheKey, response, request.Expiration ?? TimeSpan.FromMinutes(5), ct);
        return response;
    }
}