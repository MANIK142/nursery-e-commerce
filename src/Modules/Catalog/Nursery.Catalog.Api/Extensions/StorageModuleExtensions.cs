using Amazon.S3;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Infrastructure.Repository;

namespace Nursery.Catalog.Api.Extensions;

public static class StorageModuleExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration config)
    {
        var Storage = "Storage_Local";
        services.Configure<StorageOptions>(config.GetSection(Storage));
        var options = config.GetSection(Storage).Get<StorageOptions>()!;

        switch (options.Provider)
        {
            case "Azure":
                services.AddSingleton(new BlobServiceClient(options.Azure.ConnectionString));
                services.AddScoped<IBlobStorageService, AzureBlobStorageService>();
                break;

            case "S3":
                services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
                    options.S3.AccessKey, options.S3.SecretKey,
                    new AmazonS3Config
                    {
                        ServiceURL = options.S3.ServiceUrl,
                        ForcePathStyle = options.S3.ForcePathStyle,
                        AuthenticationRegion = options.S3.Region
                    }));
                services.AddScoped<IBlobStorageService, S3BlobStorageService>();
                break;

            case "Local":
                services.AddScoped<IBlobStorageService, LocalStorageService>();
                break;

            default:
                throw new InvalidOperationException($"Unknown storage provider: {options.Provider}");
        }

        return services;
    }
}

public class StorageOptions
{
    public string SectionName { get; set; } = "Local";
    public string Provider { get; set; } = "Local";
    public string? CdnBaseUrl { get; set; } = "http://localhost:9000";
    public S3Options? S3 { get; set; } = new();
    public AzureOptions? Azure { get; set; } = new();
}
public class S3Options
{
    public string ServiceUrl { get; set; } = "http://localhost:9000";
    public string AccessKey { get; set; } = "minioadmin";
    public string SecretKey { get; set; } = "minioadmin";
    public string Region { get; set; } = "us-east-1";
    public bool ForcePathStyle { get; set; } = false;
}

public class AzureOptions
{
    public string ConnectionString { get; set; } = "";
}