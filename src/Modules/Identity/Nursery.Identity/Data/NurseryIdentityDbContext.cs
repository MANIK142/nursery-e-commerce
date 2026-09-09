using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nursery.Identity.Models.Domain;

namespace Nursery.Identity.Data;

public class NurseryIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public NurseryIdentityDbContext(DbContextOptions<NurseryIdentityDbContext> options):base (options)
    {   
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var customerRoleId = "a71a55d6-99d7-4123-b4e0-1218ecb90e3e";
        var adminRoleId = "c309fa92-2123-47be-b397-a1c77adb502c";
        var empRoleId = "e6902b62-bd46-4561-950c-8c8eb63db8bc";

        var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = customerRoleId,
                    ConcurrencyStamp = customerRoleId,
                    Name = "Customer",
                    NormalizedName = "Customer".ToUpper()
                },
                new IdentityRole
                {
                    Id = empRoleId,
                    ConcurrencyStamp = empRoleId,
                    Name = "Employee",
                    NormalizedName = "Employee".ToUpper()
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper()
                }
            };

        builder.Entity<IdentityRole>().HasData(roles);
    }
}
