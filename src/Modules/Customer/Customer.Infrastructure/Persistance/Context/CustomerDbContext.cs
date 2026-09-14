
using BuildingBlocks.Common;
using Customer.Application.Data;
using Customer.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
namespace Customer.Infrastructure.Persistance.Context;
public class CustomerDbContext : DbContext, ICustomerDbContext
{
    private readonly IPublisher _publisher;

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options,IPublisher publisher):base(options)
    {
        this._publisher = publisher;
    }
    public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();
    public DbSet<Address> Addresss => Set<Address>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = ChangeTracker.Entries<BaseDomainModel>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.DomainEvents.ToList();
            entity.ClearDomainEvents();
            foreach (var domainEvent in events)
                await _publisher.Publish(domainEvent, cancellationToken);
        }

        return result;
    }
}
