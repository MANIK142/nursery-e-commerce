using BuildingBlocks.Common.CQRS;
using Customer.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Customer.Application.CustomerHandler.GetAddresses;

public record GetAddressesQuery(Guid CustomerId) : IQuery<GetAddressesResult>;

public record GetAddressesResult(List<CustomerAddressDto> CustomerAddresses);
