using Customer.Domain.Models;

namespace Customer.Application.Data;
public interface ICustomerRepository
{
    Task<CustomerEntity?> GetCustomerByEmail(string email, CancellationToken cancellationToken);
    Task<bool> CreateCustomer(CustomerEntity customer, CancellationToken cancellationToken);
    Task<bool> UpdateCustomer(CustomerEntity customer, CancellationToken cancellationToken);

}
