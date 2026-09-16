using Microsoft.AspNetCore.Identity;
using Nursery.Identity.Models.Domain;

namespace Nursery.Identity.Repository;

public interface ITokenRepository
{
    string CreateJWTToken(ApplicationUser user, Guid? customerId, List<string> roles);
}
