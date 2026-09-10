using BuildingBlocks.Common.Behaviors;
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Infrastructure.Persistance.Context;
using System.Reflection;

namespace Nursery.Orders.API.Extenstions;

public static class AddOrdersModuleExtention 
{
    public static IServiceCollection AddOrdersModule(this IServiceCollection services,IConfiguration configuration) 
    {
        services.AddDbContext<OrdersDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrdersDb")));

        services.AddScoped<IOrdersDbContext>(sp => sp.GetRequiredService<OrdersDbContext>());

        //services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        return services;
    }
}
