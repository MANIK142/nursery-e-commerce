using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace BuildingBlocks.Common.Caching;
public class MemoryCacheService(IMemoryCache cache) : ICacheService
{
    private static readonly ConcurrentDictionary<string, byte> TrackedKeys = new();

    public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        => Task.FromResult(cache.TryGetValue(key, out T? value) ? value : default);

    public Task SetAsync<T>(string key, T value, TimeSpan expiration, CancellationToken ct = default)
    {
        cache.Set(key, value, expiration);
        TrackedKeys.TryAdd(key, 0);
        return Task.CompletedTask;
    }

    public Task RemoveByPrefixAsync(string prefix, CancellationToken ct = default)
    {
        foreach (var key in TrackedKeys.Keys.Where(k => k.StartsWith(prefix, StringComparison.Ordinal)))
        {
            cache.Remove(key);
            TrackedKeys.TryRemove(key, out _);
        }
        return Task.CompletedTask;
    }
}