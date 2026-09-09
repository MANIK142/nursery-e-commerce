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

    public async Task<bool> CreateCustomer(CustomerEntity customer, CancellationToken cancellationToken)
    {
        customerDb.Customers.Add(customer);
        var affectedrows = await customerDb.SaveChangesAsync(cancellationToken);
        return affectedrows > 0;
    }

    public async Task<CustomerEntity?> GetCustomerByEmail(string email, CancellationToken cancellationToken)
    {
        return await customerDb.Customers.FirstOrDefaultAsync(c => c.EmailAddress == email,cancellationToken);
    }

    public async Task<bool> UpdateCustomer(CustomerEntity customer, CancellationToken cancellationToken)
    {
        var affectedrows = await customerDb.SaveChangesAsync(cancellationToken);
        return affectedrows > 0;
    }
}
