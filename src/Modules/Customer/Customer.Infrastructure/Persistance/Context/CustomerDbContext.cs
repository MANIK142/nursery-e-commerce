
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Customer.Domain.Models;
using Customer.Application.Data;
namespace Customer.Infrastructure.Persistance.Context;
public class CustomerDbContext : DbContext, ICustomerDbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options):base(options)
    {
        
    }
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
    public DbSet<Address> Addresss => Set<Address>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
