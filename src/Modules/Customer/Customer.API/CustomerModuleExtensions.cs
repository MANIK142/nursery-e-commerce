using Customer.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace Customer.API;

public static class CustomerModuleExtensions
{
    public static IServiceCollection AddCustomerModule(this IServiceCollection services,IConfiguration configuration)
    {
       
        services.AddDbContext<CustomerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CustomerDb")));
        return services;
    }
}
