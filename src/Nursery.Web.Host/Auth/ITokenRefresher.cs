using Nursery.Web.Host.Models.DTOs.Identity;

namespace Nursery.Web.Host.Auth;

public interface ITokenRefresher
{
    Task<LoginResponse?> RefreshAsync(string refreshToken, CancellationToken ct);
}
