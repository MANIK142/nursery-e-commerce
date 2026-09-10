using BuildingBlocks.Common.CQRS;
using Customer.Application.Data;
using Microsoft.EntityFrameworkCore;
namespace Customer.Application.CustomerHandler.GetCustomer;
public class GetCustomerHandler(ICustomerDbContext customerDbContext) : IQueryHandler<GetCustomerQuery, GetCustomerResult>
{
    private readonly ICustomerDbContext context = customerDbContext;

    public async Task<GetCustomerResult> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        if (request.Id != null)
        {
            var customers = await context.Customers
                              .Where(p => p.Id == request.Id)
                              .Include(p => p.Addresses)
                              .ToListAsync();

            return new GetCustomerResult(customers);
        }

        var query = context.Customers
                    .Include(p => p.Addresses).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.FilterValue) && !string.IsNullOrWhiteSpace(request.FilterBy))
        {
            if (request.FilterBy?.ToLower() == "firstname")
            {
                query = query.Where(c => c.FirstName != null && c.FirstName.Contains(request.FilterValue));
            }
            else if (request.FilterBy?.ToLower() == "lastname")
            {
                query = query.Where(c => c.LastName != null && c.LastName.Contains(request.FilterValue));
            }
            else if (request.FilterBy?.ToLower() == "city")
            {
                query = query.Where(c => c.Addresses.Any(a => a.City.Contains(request.FilterValue)));
            }
            else if (request.FilterBy?.ToLower() == "state")
            {
                query = query.Where(c => c.Addresses.Any(a => a.State.Contains(request.FilterValue)));
            }
            else if (request.FilterBy?.ToLower() == "country")
            {
                query = query.Where(c => c.Addresses.Any(a => a.Country.Contains(request.FilterValue)));
            }
        }

        var pageNumber = request.PageNumber ?? 0;
        var pageSize = request.PageSize ?? 10;

        var FilteredCustomers = await query
                  .OrderBy(c => c.CreatedAt)
                  .Skip(pageNumber * pageSize)
                  .Take(pageSize)
                  .ToListAsync();
        return new GetCustomerResult(FilteredCustomers);
    }
}
