using Nursery.Web.Host.Models.DTOs.Customer;

namespace Nursery.Web.Host.Services.Interface;

public interface ICustomerApi 
{
    async Task<AddressDto> GetCustomerAddresses(CancellationToken cancellation);
}
