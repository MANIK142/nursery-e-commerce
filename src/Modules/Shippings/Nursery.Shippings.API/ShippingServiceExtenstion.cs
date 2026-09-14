using Carter;
using Microsoft.EntityFrameworkCore;
using Nursery.Shippings.Application;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Application.Endpoints;
using Nursery.Shippings.Infrastucture.Presistance.Context;
using Nursery.Shippings.Infrastucture.Presistance.Repository;
namespace Nursery.Shippings.API;

public static class ShippingServiceExtenstion
{ 
    public static IServiceCollection AddShippingService(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddApplicationServices(configuration);

        services.AddCarter(configurator: c =>
        {
            c.WithModule<ShipmentEndpoints>();
        });

        services.AddDbContext<ShippingDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ShippingDb")));

        services.AddScoped<IShippingDbContext>(sp => sp.GetRequiredService<ShippingDbContext>());

        services.AddScoped<IShipmentRepository, ShipmentRepository>();

        return services;
    }

}
