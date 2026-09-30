using Nursery.Identity.Models.Domain;
using Nursery.Identity.Models.DTO;

namespace Nursery.Identity.Services;

public interface IAuthTokenService
{
    Task<LoginResponseDto> IssueAsync(ApplicationUser user, Guid familyId, DateTime familyCreatedAtUtc, CancellationToken ct);

    Task<LoginResponseDto> RefreshTokenAsync(string RefreshToken,CancellationToken ct);
}