using BuildingBlocks.Common.Behaviors;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using Nursery.Catalog.Infrastructure.Repository;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Application;
var builder = WebApplication.CreateBuilder(args);


builder.Services
    .AddApplicationServices(builder.Configuration);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCarter();

//MediatR and FluentValidation Configuration
var Assembly = typeof(Program).Assembly;
builder.Services.AddValidatorsFromAssembly(Assembly);

//DbContext Configuration
builder.Services.AddDbContext<CatalogDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("catalogDb")));

builder.Services.AddScoped<ICatalogDbContext, CatalogDbContext>();
builder.Services.AddScoped<ICatalogRepository,CatalogRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
}
app.MapCarter();
app.UseHttpsRedirection();

app.Run();
