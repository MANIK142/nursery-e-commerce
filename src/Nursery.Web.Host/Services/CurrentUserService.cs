using Nursery.Web.Host.Services.Interface;
using System.Security.Claims;

namespace Nursery.Web.Host.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _accessor;
    public CurrentUserService(IHttpContextAccessor accessor) => _accessor = accessor;

    private ClaimsPrincipal? User => _accessor.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;
    public string? CustomerId => User?.FindFirst("customer_id")?.Value;
    public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;
    public string? Name => User?.FindFirst(ClaimTypes.Name)?.Value;
}
