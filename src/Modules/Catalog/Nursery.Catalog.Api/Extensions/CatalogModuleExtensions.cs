
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nursery.Catalog.Application;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using Nursery.Catalog.Infrastructure.Repository;

namespace Nursery.Catalog.Api.Extensions;

public static class CatalogModuleExtensions
{
    public static IServiceCollection AddCatalogModules(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices(configuration);

        var assembly = typeof(CatalogModuleExtensions).Assembly;
        services.AddValidatorsFromAssembly(assembly);

        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("catalogDb")));

        services.AddScoped<ICatalogDbContext, CatalogDbContext>();
        services.AddScoped<ICatalogRepository, CatalogRepository>();

        services.AddStorage(configuration);
        return services;
    }
}
