
using BuildingBlocks.Common.CQRS;
using Customer.Application.Data;
using Customer.Domain.Models;
using Nursery.Catalog.Application.Exceptions;
using System.Windows.Input;

namespace Customer.Application.CustomerHandler.CreateCustomer;

public class CreateCustomerHandler(ICustomerRepository customerRepository) : ICommandHandler<CreateCustomerCommand,CreateCustomerResult>
{
    private readonly ICustomerRepository customerRepository = customerRepository;

    public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var ExistingCustomer = customerRepository.GetCustomerByEmail(request.EmailAddress, cancellationToken);
        if (ExistingCustomer != null)
            throw new CustomerAlreadyExistsException($"Customer already Exists with this email address {request.EmailAddress}");

        var customer = CustomerEntity.Create(request.FirstName, request.LastName, request.ExternalUserId, request.EmailAddress, request.PhoneNumber);
    
        var result = await customerRepository.CreateCustomer(customer,cancellationToken);
        return new CreateCustomerResult(result);

    }
}
