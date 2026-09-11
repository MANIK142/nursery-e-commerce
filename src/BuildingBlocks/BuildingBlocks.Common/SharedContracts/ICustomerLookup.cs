using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingBlocks.Common.SharedContracts;

public interface ICustomerLookup
{
    Task<Guid?> GetCustomerIdByExternalUserIdAsync(string externalUserId, CancellationToken ct);
}
