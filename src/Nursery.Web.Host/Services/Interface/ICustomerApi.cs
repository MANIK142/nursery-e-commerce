using Nursery.Web.Host.Models.DTOs.Customer;

namespace Nursery.Web.Host.Services.Interface;

public interface ICustomerApi 
{
     Task<List<AddressDto>> GetCustomerAddresses(CancellationToken cancellation);
}
