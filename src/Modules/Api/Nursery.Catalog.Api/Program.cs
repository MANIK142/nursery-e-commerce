using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Util;
using BuildingBlocks.Common.Behaviors;
using BuildingBlocks.Exceptions.Handler;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Nursery.Catalog.Application;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using Nursery.Catalog.Infrastructure.Repository;
using Scalar.AspNetCore;

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

//builder.Services.AddScoped<IBlobStorageService,LocalStorageService>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();


var AccessKey = builder.Configuration.GetValue<string>("Storage_Aws:S3:AccessKey");
var SecretKey = builder.Configuration.GetValue<string>("Storage_Aws:S3:SecretKey");
var ServiceUrl = builder.Configuration.GetValue<string>("Storage_Aws:S3:ServiceUrl");
var ForcePathStyle = builder.Configuration.GetValue<bool>("Storage_Aws:S3:ForcePathStyle");
var AuthenticationRegion = builder.Configuration.GetValue<string>("Storage_Aws:S3:Region");
builder.Services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
                AccessKey, SecretKey,
                new AmazonS3Config
                {
                    ServiceURL = ServiceUrl,
                    ForcePathStyle = ForcePathStyle,
                    AuthenticationRegion = AuthenticationRegion
                }));
builder.Services.AddScoped<IBlobStorageService, S3BlobStorageService>();


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
