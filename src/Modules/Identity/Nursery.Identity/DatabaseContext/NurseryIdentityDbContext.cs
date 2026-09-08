using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nursery.Identity.Models;

namespace Nursery.Identity.DatabaseContext;

public class NurseryIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public NurseryIdentityDbContext(DbContextOptions<NurseryIdentityDbContext> options):base (options)
    {   
    }
}
