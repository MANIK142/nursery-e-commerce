
using Customer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Customer.Application.Data;
public interface ICustomerDbContext
{
    DbSet<CustomerEntity> Customers { get; }
    DbSet<Address> Addresss { get; }
}
