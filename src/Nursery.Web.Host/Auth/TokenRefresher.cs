using Microsoft.Extensions.Caching.Memory;
using Nursery.Web.Host.Models.DTOs.Identity;
using Nursery.Web.Host.Services.Interface;
using System.Security.Cryptography;
using System.Text;

namespace Nursery.Web.Host.Auth;

public sealed class TokenRefresher(IIdentityApiClient api, IMemoryCache cache) : ITokenRefresher
{
    private static readonly SemaphoreSlim Gate = new(1, 1);   // single instance; use a distributed lock when multi-pod
    private static readonly TimeSpan ResultTtl = TimeSpan.FromSeconds(30);

    public async Task<LoginResponse?> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        var key = "refresh:" + Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
        if (cache.TryGetValue(key, out LoginResponse? cached)) return cached;

        await Gate.WaitAsync(ct);
        try
        {
            if (cache.TryGetValue(key, out cached)) return cached;   // another request refreshed while we waited

            var result = await api.RefreshAsync(refreshToken, ct);
            if (result is not null) cache.Set(key, result, ResultTtl);
            return result;
        }
        finally { Gate.Release(); }
    }
}
