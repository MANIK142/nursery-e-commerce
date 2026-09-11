using BuildingBlocks.Common.SharedContracts;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using Customer.API;
using Customer.Infrastructure.Persistance.Repository;
using Microsoft.AspNetCore.Authorization;
using Nursery.Catalog.Api.Extensions;
using Nursery.Identity;
using Nursery.Orders.API.Endpoints;
using Nursery.Orders.API.Extenstions;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Infrastructure.Persistance.Repository;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddApplicationPart(typeof(Nursery.Identity.IdentityExtensions).Assembly)
    .AddApplicationPart(typeof(Customer.API.CustomerModuleExtensions).Assembly);

builder.Services.AddOpenApi();

builder.Services.AddCarter();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();


builder.Services.AddCatalogModules(builder.Configuration);
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddCustomerModule(builder.Configuration);
builder.Services.AddOrdersModule(builder.Configuration);

builder.Services.AddScoped<ICustomerLookup, CustomerLookup>();
builder.Services.AddScoped<ICatalogLookup, CatalogLookup>();

var app = builder.Build();

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


app.Run();
