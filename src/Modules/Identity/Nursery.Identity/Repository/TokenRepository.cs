using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Nursery.Identity.Models.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Nursery.Identity.Repository;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration configuration;
    public TokenRepository(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public string CreateJWTToken(ApplicationUser user,Guid? customerId, List<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(ClaimTypes.Name, user.FirstName));

        if (customerId != null)
        {
            claims.Add(new Claim("customer_id", customerId.ToString()));
            claims.Add(new Claim("last_name", user.LastName));
        }
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        int validityInSeconds = 10;
        if (int.TryParse(configuration["Jwt:ValidityInSeconds"], out int validity))
        {
            validityInSeconds = validity;
        }
  
        var token = new JwtSecurityToken(configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddSeconds(validityInSeconds),
                signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
