
namespace BuildingBlocks.Common.Caching;

public interface ICacheInvalidatorCommand
{
    string CacheKeyPrefix { get; }
}
