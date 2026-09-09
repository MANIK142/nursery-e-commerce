using Customer.Application;
using Customer.Application.Data;
using Customer.Infrastructure.Persistance.Context;
using Customer.Infrastructure.Persistance.Repository;
using Microsoft.EntityFrameworkCore;

namespace Customer.API;

public static class CustomerModuleExtensions
{
    public static IServiceCollection AddCustomerModule(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddApplicationServices(configuration);
        services.AddDbContext<CustomerDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CustomerDb")));

        services.AddScoped<ICustomerDbContext, CustomerDbContext>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        return services;
    }
}
