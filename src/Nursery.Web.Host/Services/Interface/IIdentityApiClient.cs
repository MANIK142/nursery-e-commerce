using Nursery.Web.Host.Models;
using Nursery.Web.Host.Models.DTOs.Identity;

namespace Nursery.Web.Host.Services.Interface;

public interface IIdentityApiClient
{
    Task<ApiResultModel<LoginResponse>?> LoginAync(LoginRequest loginRequest,CancellationToken ct);

    Task<ApiResultModel<RegisterResponse>?> RegisterAsync(RegisterRequest registerRequest, CancellationToken ct);
}
