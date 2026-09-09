using BuildingBlocks.Exceptions.Handler;
using Carter;
using Customer.API;
using Microsoft.AspNetCore.Authorization;
using Nursery.Catalog.Api.Extensions;
using Nursery.Identity;
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
