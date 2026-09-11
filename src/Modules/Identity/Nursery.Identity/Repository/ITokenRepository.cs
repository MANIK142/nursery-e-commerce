using Microsoft.AspNetCore.Identity;

namespace Nursery.Identity.Repository;

public interface ITokenRepository
{
    string CreateJWTToken(IdentityUser user, Guid? customerId, List<string> roles);
}
