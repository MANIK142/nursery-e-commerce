using System.Security.Claims;

namespace BuildingBlocks.Common;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetCustomerId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst("customer_id")?.Value
            ?? throw new UnauthorizedAccessException("Token is missing the customer_id claim.");

        return Guid.Parse(value);
    }
}
