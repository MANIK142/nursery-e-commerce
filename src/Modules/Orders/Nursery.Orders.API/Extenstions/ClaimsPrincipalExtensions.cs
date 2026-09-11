using System.Security.Claims;

namespace Nursery.Orders.API.Extenstions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetCustomerId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst("customer_id")?.Value
            ?? throw new UnauthorizedAccessException("Token is missing the customer_id claim.");

        return Guid.Parse(value);
    }
}
