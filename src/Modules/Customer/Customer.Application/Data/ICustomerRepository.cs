using Customer.Domain.Models;

namespace Customer.Application.Data;
public interface ICustomerRepository
{
    Task<CustomerEntity?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken);
    Task<CustomerEntity?> GetCustomerByIdAsync(Guid Id, CancellationToken cancellationToken);
    Task<bool> CreateCustomerAsync(CustomerEntity customer, CancellationToken cancellationToken);
    Task<bool> UpdateCustomerAsync(CustomerEntity customer, CancellationToken cancellationToken);

}
