
using BuildingBlocks.Common.CQRS;

namespace Customer.Application.CustomerHandler.RemoveAddress;

public record RemoveAddressCommand(Guid CustomerId,Guid Id) : ICommand<RemoveAddressResult>;
public record RemoveAddressResult(bool IsSuccess);
