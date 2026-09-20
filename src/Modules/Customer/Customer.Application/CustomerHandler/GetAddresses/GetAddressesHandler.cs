using BuildingBlocks.Common.CQRS;
using Customer.Application.Data;
using Customer.Application.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Customer.Application.CustomerHandler.GetAddresses;

public class GetAddressesHandler(ICustomerDbContext customerDb) : IQueryHandler<GetAddressesQuery, GetAddressesResult>
{
    public async Task<GetAddressesResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var Customer = await customerDb.Customers.Include(c => c.Addresses).FirstOrDefaultAsync( c => c.Id == request.CustomerId);
        List<CustomerAddressDto> customerAddresses = new List<CustomerAddressDto>();
        foreach (var Address in Customer.Addresses)
        {
            var address = new CustomerAddressDto(Customer.ExternalUserId, Address.Type, Address.Line1, Address.Line2, Address.City, Address.State, Address.PostalCode, Address.Country, Address.IsDefault);
            customerAddresses.Add(address);
         }

        return new GetAddressesResult(customerAddresses);
    }
}
