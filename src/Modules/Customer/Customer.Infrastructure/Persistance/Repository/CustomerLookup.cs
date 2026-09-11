using BuildingBlocks.Common.SharedContracts;
using Customer.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Customer.Infrastructure.Persistance.Repository;

public class CustomerLookup(CustomerDbContext dbContext) : ICustomerLookup
{
    public async Task<Guid?> GetCustomerIdByExternalUserIdAsync(string externalUserId, CancellationToken ct)
    {
        return await dbContext.Customers
            .AsNoTracking()
            .Where(c => c.ExternalUserId == externalUserId)
            .Select(c => (Guid?)c.Id)
            .FirstOrDefaultAsync(ct);
    }
}
