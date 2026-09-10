using BuildingBlocks.Common.CQRS;
using Customer.Domain.Models;
namespace Customer.Application.CustomerHandler.GetCustomer;
public record GetCustomerQuery(int? PageNumber, int? PageSize, Guid? Id, string? FilterBy, string? FilterValue)
    : IQuery<GetCustomerResult>;
public record GetCustomerResult(List<CustomerEntity> Customers);


