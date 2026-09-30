using Microsoft.EntityFrameworkCore;
using Nursery.Identity.Data;
using Nursery.Identity.Models.Domain;

namespace Nursery.Identity.Repository;

public sealed class RefreshTokenRepository(NurseryIdentityDbContext db) : IRefreshTokenRepository   
{
    public Task<RefreshToken?> GetByHashAsync(string tokenHash, CancellationToken ct) =>
        db.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(t => t.TokenHash == tokenHash, ct);

    public async Task AddAsync(RefreshToken token, CancellationToken ct) =>
        await db.RefreshTokens.AddAsync(token, ct);

    // Atomic claim: only one concurrent caller can flip UsedAtUtc from null; the loser gets false.
    public async Task<bool> TryMarkUsedAsync(Guid id, DateTime nowUtc, CancellationToken ct) =>
        await db.RefreshTokens
            .Where(t => t.Id == id && t.UsedAtUtc == null && t.RevokedAtUtc == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.UsedAtUtc, nowUtc), ct) == 1;

    public Task RevokeFamilyAsync(Guid familyId, DateTime nowUtc, CancellationToken ct) =>
        db.RefreshTokens
            .Where(t => t.FamilyId == familyId && t.RevokedAtUtc == null)
            .ExecuteUpdateAsync(s => s.SetProperty(t => t.RevokedAtUtc, nowUtc), ct);

    public Task<int> SaveChangesAsync(CancellationToken ct) => db.SaveChangesAsync(ct);
}
