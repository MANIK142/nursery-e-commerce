using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using Azure.Storage.Blobs;
using BuildingBlocks.Common.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using FluentValidation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nursery.Catalog.Api.Extensions;
using Nursery.Catalog.Application;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using Nursery.Catalog.Infrastructure.Repository;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

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
    options.UseSqlServer(builder.Configuration.GetConnectionString("catalogDb")
    ));

builder.Services.AddScoped<ICatalogDbContext, CatalogDbContext>();
builder.Services.AddScoped<ICatalogRepository,CatalogRepository>();


builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


builder.Services.AddStorage(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapCarter();
app.UseExceptionHandler(options => { });
app.UseHttpsRedirection();


app.Run();
