using BuildingBlocks.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursery.Payment.Api.Data.Interface;
using Nursery.Payment.Api.Models;

namespace Nursery.Payment.Api.Persistance;

public class PaymentDbContext : DbContext, IPaymentbContext
{
    private readonly IPublisher _publisher;

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options,IPublisher publisher) :base(options)
    {
        this._publisher = publisher;
    }

    public DbSet<Models.Payment> Payments => Set<Models.Payment>();

    public DbSet<PaymentAttempt> PaymentAttempts => Set<PaymentAttempt>();

    public DbSet<ProcessedWebhookEvent> ProcessedWebhookEvents => Set<ProcessedWebhookEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentDbContext).Assembly);
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
