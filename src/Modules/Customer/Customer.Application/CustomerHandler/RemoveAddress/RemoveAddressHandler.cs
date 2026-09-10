
using BuildingBlocks.Common.CQRS;
using Customer.Application.Data;
using Nursery.Catalog.Application.Exceptions;

namespace Customer.Application.CustomerHandler.RemoveAddress;

public class RemoveAddressHandler(ICustomerRepository repository) : ICommandHandler<RemoveAddressCommand, RemoveAddressResult>
{
    private readonly ICustomerRepository repository = repository;

    public async Task<RemoveAddressResult> Handle(RemoveAddressCommand request, CancellationToken cancellationToken)
    {
        var Customer = await repository.GetCustomerByIdAsync(request.CustomerId, cancellationToken)
                           ?? throw new CustomerNotFoundException($"Customer for found provided CustomerId {request.CustomerId}");

        Customer.RemoveAddress(request.Id);
        var result = await repository.UpdateCustomerAsync(Customer, cancellationToken);
        return new RemoveAddressResult(result);
    }
}
