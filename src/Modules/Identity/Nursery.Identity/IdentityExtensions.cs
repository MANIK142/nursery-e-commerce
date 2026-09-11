using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nursery.Identity.Data;
using Nursery.Identity.Models.Domain;
using Nursery.Identity.Repository;
using System.Reflection;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BuildingBlocks.Common.SharedContracts;
namespace Nursery.Identity;

public static class IdentityExtensions
{
    public static IServiceCollection  AddIdentityModule(this IServiceCollection services,IConfiguration configuration)
    {
       
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddDbContext<NurseryIdentityDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("IdentityDb"));
        });

        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;              // Must have at least one number (0-9)
            options.Password.RequireLowercase = true;          // Must have at least one lowercase letter (a-z)
            options.Password.RequireUppercase = true;          // Must have at least one uppercase letter (A-Z)
            options.Password.RequireNonAlphanumeric = true;    // Must have at least one special character (!@#$%^&*)
            options.Password.RequiredLength = 7;               // Minimum 7 characters
            options.Password.RequiredUniqueChars = 1;          // Minimum unique characters
        })
        .AddEntityFrameworkStores<NurseryIdentityDbContext>();

        services.AddScoped<ITokenRepository, TokenRepository>();
      
        int validityInSeconds = 10;
        if (int.TryParse(configuration["Jwt:ValidityInSeconds"], out int validity))
        {
            validityInSeconds = validity;
        }

        var jwtKey = configuration["Jwt:Key"]!;
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience =configuration["Jwt:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMilliseconds(validityInSeconds)
            };
        });



        services.AddAuthorization();
        services.AddHttpContextAccessor();

        return services;
    }
}
