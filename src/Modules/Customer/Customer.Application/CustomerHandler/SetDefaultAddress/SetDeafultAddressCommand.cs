using BuildingBlocks.Common.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Customer.Application.CustomerHandler.SetDefaultAddress;

public record SetDeafultAddressCommand(Guid CustomerId, Guid Id) : ICommand<SetDeafultAddressResult>;

public record SetDeafultAddressResult(bool IsSuccess);
