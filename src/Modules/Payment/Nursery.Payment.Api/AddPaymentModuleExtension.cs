using BuildingBlocks.Common.Behaviors;
using Carter;
using Microsoft.EntityFrameworkCore;
using Nursery.Payment.Api.Contracts;
using Nursery.Payment.Api.Endpoints;
using Nursery.Payment.Api.Gateways;
using Nursery.Payment.Api.Persistance;
using Nursery.Payment.Api.Persistance.Repository;
using System.Reflection;


namespace Nursery.Payment.Api;

public static class AddPaymentModuleExtension 
{
    public static IServiceCollection AddPaymentModule(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddCarter(configurator: c =>
        {
            c.WithModule<PaymentEndpoints>();
        });

        services.AddDbContext<PaymentDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("PaymentDb"));
        });



        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IProcessedWebhookEventRepository, ProcessedWebhookEventRepository>();
        services.Configure<StripeOptions>(configuration.GetSection(StripeOptions.SectionName));
        services.AddScoped<IPaymentGatewayService, StripePaymentGatewayService>();


        return services;
    }
}
