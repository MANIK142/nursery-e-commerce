using BuildingBlocks.Common.SharedContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Nursery.Identity.Models.Domain;
using Nursery.Identity.Models.DTO;
using Nursery.Identity.Repository;

namespace Nursery.Identity.Services;

public sealed class AuthTokenService(
    UserManager<ApplicationUser> userManager,
    ICustomerLookup customerLookup,
    ITokenRepository tokenRepository,
    IRefreshTokenRepository refreshTokens,
    IConfiguration config) : IAuthTokenService
{
    public async Task<LoginResponseDto> IssueAsync(
        ApplicationUser user, Guid familyId, DateTime familyCreatedAtUtc, CancellationToken ct)
    {
        var roles = await userManager.GetRolesAsync(user);
        var customerId = await customerLookup.GetCustomerIdByExternalUserIdAsync(user.Id, ct);

        var jwt = tokenRepository.CreateJWTToken(user, customerId, roles.ToList());
        var expiresAtUtc = new JsonWebTokenHandler().ReadJsonWebToken(jwt).ValidTo;   // the token's real exp

        var (raw, hash) = RefreshTokenGenerator.Create();
        var lifetime = TimeSpan.FromDays(config.GetValue("Jwt:RefreshTokenDays", 30));
        await refreshTokens.AddAsync(
            RefreshToken.Create(Guid.Parse(user.Id), familyId, familyCreatedAtUtc, hash, DateTime.UtcNow, lifetime), ct);

        return new LoginResponseDto { JwtToken = jwt, ExpiresAtUtc = expiresAtUtc, RefreshToken = raw };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string RefreshToken, CancellationToken ct)
    {
        var existing = await refreshTokens.GetByHashAsync(RefreshTokenGenerator.Hash(RefreshToken), ct);
        if (existing is null) return null;

        var now = DateTime.UtcNow;

        // Replay of an already-rotated/revoked token => assume theft, kill the whole session
        if (existing.UsedAtUtc is not null || existing.RevokedAtUtc is not null)
        {
            await refreshTokens.RevokeFamilyAsync(existing.FamilyId, now, ct);
            return null;
        }

        if (existing.ExpiresAtUtc <= now) return null;

        // Absolute session cap so rolling can't extend a session forever
        var absoluteDays = config.GetValue("Jwt:RefreshTokenAbsoluteDays", 30);
        if (existing.FamilyCreatedAtUtc.AddDays(absoluteDays) <= now)
        {
            await refreshTokens.RevokeFamilyAsync(existing.FamilyId, now, ct);
            return null;
        }

        // Atomic claim: if a concurrent request got there first, treat as replay
        if (!await refreshTokens.TryMarkUsedAsync(existing.Id, now, ct))
        {
            await refreshTokens.RevokeFamilyAsync(existing.FamilyId, now, ct);
            return null;
        }

        var user = await userManager.FindByIdAsync(existing.UserId.ToString());
        if (user is null || await userManager.IsLockedOutAsync(user))
        {
            await refreshTokens.RevokeFamilyAsync(existing.FamilyId, now, ct);
            return null;
        }

        var response = await IssueAsync(user, existing.FamilyId, existing.FamilyCreatedAtUtc, ct);
        await refreshTokens.SaveChangesAsync(ct);
        return response;
    }
}