using BuildingBlocks.Common.CQRS;
using Customer.Application.Data;
using Customer.Application.Dtos;
using Customer.Domain.Models;
using Nursery.Catalog.Application.Exceptions;
using System.Runtime.CompilerServices;

namespace Customer.Application.CustomerHandler.UpdateCustomer;

public class UpdateCustomerHandler(ICustomerRepository repository) : ICommandHandler<UpdateCustomerProfileCommand, UpdateCustomerProfileResult>
{
    private readonly ICustomerRepository repository = repository;

    public async Task<UpdateCustomerProfileResult> Handle(UpdateCustomerProfileCommand request, CancellationToken cancellationToken)
    {

        var Customer = await repository.GetCustomerByEmailAsync(request.Email, cancellationToken)
                            ??  throw new CustomerNotFoundException($"Customer for found provided email address {request.Email}");

        Customer.UpdateProfile(request.FirstName, request.LastName, request.PhoneNumber);

        if (request.AddressDtos?.Count > 0)
        {
            foreach (var addressDto in request.AddressDtos) {
                var IsExists = CheckAddressExists(addressDto, Customer.Addresses.ToList());
                if (!IsExists)
                {
                    var address = Customer.AddAddress(addressDto.Type, addressDto.Line1, addressDto.Line2
                        , addressDto.City, addressDto.State, addressDto.PostalCode, addressDto.Country,addressDto.IsDefault);
                }
            }
        }
        var result = await repository.UpdateCustomerAsync(Customer, cancellationToken);

        return new UpdateCustomerProfileResult(result);
    }

    public bool CheckAddressExists(CustomerAddressDto addressDto, List<Address> addresses)
    {
        foreach (var address in addresses)
        {
            if (address.Line1 == addressDto.Line1 && address.Line2 == addressDto.Line2
                && address.City == addressDto.City && address.State == addressDto.State && address.PostalCode == addressDto.PostalCode
                && address.Country == addressDto.Country)
            {
                return true;
            }
        }
        return false;
    }
}
