using Nursery.Identity.Models.Domain;

namespace Nursery.Identity.Repository;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct);
    Task AddAsync(RefreshToken token, CancellationToken ct);
    Task<bool> TryMarkUsedAsync(Guid id, DateTime nowUtc, CancellationToken ct);
    Task RevokeFamilyAsync(Guid familyId, DateTime nowUtc, CancellationToken ct);
    Task<int> SaveChangesAsync(CancellationToken ct);
}
