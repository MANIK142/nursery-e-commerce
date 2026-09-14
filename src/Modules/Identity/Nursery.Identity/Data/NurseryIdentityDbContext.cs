using BuildingBlocks.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nursery.Identity.Models.Domain;

namespace Nursery.Identity.Data;

public class NurseryIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly IPublisher _publisher;

    public NurseryIdentityDbContext(DbContextOptions<NurseryIdentityDbContext> options,IPublisher publisher):base (options)
    {
        this._publisher = publisher;
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var customerRoleId = "a71a55d6-99d7-4123-b4e0-1218ecb90e3e";
        var adminRoleId = "c309fa92-2123-47be-b397-a1c77adb502c";
        var empRoleId = "e6902b62-bd46-4561-950c-8c8eb63db8bc";
        var WarehouseStaffId = "3ca074b1-81cf-470e-99d3-b652a54dafad";
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
                },
                 new IdentityRole
                {
                    Id = WarehouseStaffId,
                    ConcurrencyStamp = WarehouseStaffId,
                    Name = "WarehouseStaff",
                    NormalizedName = "WarehouseStaff".ToUpper()
                },
                
            };

        builder.Entity<IdentityRole>().HasData(roles);
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
