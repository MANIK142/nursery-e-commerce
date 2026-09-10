using Customer.Application.Data;
using Customer.Domain.Models;
using Customer.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Customer.Infrastructure.Persistance.Repository;

public class CustomerRepository(CustomerDbContext customerDb) : ICustomerRepository
{
    private readonly CustomerDbContext customerDb = customerDb;

    public async Task<bool> CreateCustomerAsync(CustomerEntity customer, CancellationToken cancellationToken)
    {
        customerDb.Customers.Add(customer);
        var affectedrows = await customerDb.SaveChangesAsync(cancellationToken);
        return affectedrows > 0;
    }

    public async Task<CustomerEntity?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await customerDb.Customers.Include(c => c.Addresses).FirstOrDefaultAsync(c => c.EmailAddress == email,cancellationToken);
    }

    public async Task<CustomerEntity?> GetCustomerByIdAsync(Guid Id, CancellationToken cancellationToken)
    {
        return await customerDb.Customers.Include(c => c.Addresses).FirstOrDefaultAsync(c => c.Id == Id, cancellationToken);
    }

    public async Task<bool> UpdateCustomerAsync(CustomerEntity customer, CancellationToken cancellationToken)
    {
        customerDb.ChangeTracker.DetectChanges();
        var entry = customerDb.Entry(customer);


        foreach (var e in customerDb.ChangeTracker.Entries())
        {
            Console.WriteLine($"{e.Entity.GetType().Name} => {e.State}");
        }
        if (entry.State == EntityState.Detached)
        {
            customerDb.Customers.Attach(customer);
            entry.State = EntityState.Modified; // force EF to treat every scalar as changed

            foreach (var address in customer.Addresses)
            {
                var addressEntry = customerDb.Entry(address);
                addressEntry.State = addressEntry.IsKeySet
                    ? EntityState.Modified
                    : EntityState.Added;
            }
        }


        var affectedrows = await customerDb.SaveChangesAsync(cancellationToken);
        return affectedrows > 0;
    }
}
