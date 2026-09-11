using BuildingBlocks.Common.Behaviors;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.API.Endpoints;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Infrastructure.Persistance.Context;
using Nursery.Orders.Infrastructure.Persistance.Repository;
using System.Reflection;
using Nursery.Orders.Application;
namespace Nursery.Orders.API.Extenstions;

public static class AddOrdersModuleExtention 
{
    public static IServiceCollection AddOrdersModule(this IServiceCollection services,IConfiguration configuration) 
    {

        services.AddApplicationServices(configuration);
        services.AddCarter(configurator: c =>
        {
            c.WithModule<CartEndpoints>();
            c.WithModule<OrderEndpoints>();
        });

        services.AddDbContext<OrdersDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("OrdersDb")));

        services.AddScoped<IOrdersDbContext>(sp => sp.GetRequiredService<OrdersDbContext>());

        services.AddScoped<ICartRepository, CartReposiotry>();
        services.AddScoped<IOrderRepository, OrderRepository>();


        return services;
    }
}
