using Asp.Versioning;
using BuildingBlocks.Common.Caching;
using BuildingBlocks.Common.Middleware;
using BuildingBlocks.Common.SharedContracts;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Customer.API;
using Customer.Infrastructure.Persistance.Context;
using Customer.Infrastructure.Persistance.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Nursery.Catalog.Api.Extensions;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using Nursery.Identity;
using Nursery.Identity.Data;
using Nursery.Orders.API.Endpoints;
using Nursery.Orders.API.Extenstions;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Infrastructure.Persistance.Context;
using Nursery.Orders.Infrastructure.Persistance.Repository;
using Nursery.Payment.Api;
using Nursery.Payment.Api.Contracts;
using Nursery.Payment.Api.Persistance;
using Nursery.Shippings.API;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Infrastucture.Presistance.Context;
using Scalar.AspNetCore;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);


builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services));


builder.Services.AddControllers()
    .AddApplicationPart(typeof(Nursery.Identity.IdentityExtensions).Assembly)
    .AddApplicationPart(typeof(Customer.API.CustomerModuleExtensions).Assembly)
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    }); ;

builder.Services.AddOpenApi();

builder.Services.AddCarter();


builder.Services.AddMemoryCache();
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();


builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();


builder.Services.AddCatalogModules(builder.Configuration);
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddCustomerModule(builder.Configuration);
builder.Services.AddOrdersModule(builder.Configuration);
builder.Services.AddPaymentModule(builder.Configuration);
builder.Services.AddShippingService(builder.Configuration);



builder.Services.AddScoped<ICustomerLookup, CustomerLookup>();
builder.Services.AddScoped<ICatalogLookup, CatalogLookup>();
builder.Services.AddScoped<IOrderLookup, OrderLookup>();
builder.Services.AddScoped<IOrderLineItemLookup, OrderLineItemLookup>();

builder.Services.AddDirectoryBrowser();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true; // adds api-supported-versions header to responses
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
})
.AddMvc();


builder.Services.AddHealthChecks()
    .AddDbContextCheck<CatalogDbContext>("Nursery.CatalogDb", tags: ["ready"])
    .AddDbContextCheck<NurseryIdentityDbContext>("Nursery.IdentityDb", tags: ["ready"])
    .AddDbContextCheck<CustomerDbContext>("Nursery.CustomerDb", tags: ["ready"])
    .AddDbContextCheck<OrdersDbContext>("Nursery.OrdersDb", tags: ["ready"])
    .AddDbContextCheck<PaymentDbContext>("Nursery.PaymentDb", tags: ["ready"])
    .AddDbContextCheck<ShippingDbContext>("Nursery.ShippingDb", tags: ["ready"]);


var app = builder.Build();

app.UseCorrelationId();
//app.UseSerilogRequestLogging();
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("CorrelationId", httpContext.TraceIdentifier);
    };
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHttpsRedirection();



app.MapControllers();

var basePath = builder.Configuration.GetValue<string>("Storage_Local:BasePath");
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, basePath)),
    RequestPath = $"/{basePath}"
});

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append(
            "Cache-Control", "public,max-age=600");
    }
});
app.UseDirectoryBrowser();

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false // matches nothing — confirms the process can respond, checks nothing else
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(result);
    }
});

app.Run();
