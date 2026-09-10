
using BuildingBlocks.Common.CQRS;
using Customer.Application.CustomerHandler.RemoveAddress;
using Customer.Application.Data;
using Nursery.Catalog.Application.Exceptions;

namespace Customer.Application.CustomerHandler.SetDefaultAddress;

public class SetDefaultAddressHandler(ICustomerRepository repository) : ICommandHandler<SetDeafultAddressCommand, SetDeafultAddressResult>
{
    private readonly ICustomerRepository repository = repository;
    public async Task<SetDeafultAddressResult> Handle(SetDeafultAddressCommand request, CancellationToken cancellationToken)
    {
        var Customer = await repository.GetCustomerByIdAsync(request.CustomerId, cancellationToken)
                           ?? throw new CustomerNotFoundException($"Customer for found provided CustomerId {request.CustomerId}");

        Customer.SetDefaultAddress(request.Id);
        var result = await repository.UpdateCustomerAsync(Customer, cancellationToken);
        return new SetDeafultAddressResult(result);
    }
}
