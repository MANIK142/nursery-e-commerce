

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Infrastructure.Exceptions;

namespace Nursery.Catalog.Infrastructure.Repository;

public class LocalStorageService : IBlobStorageService
{
    private readonly IConfiguration _configuration;
    private readonly string _basePath;

    private readonly IHttpContextAccessor _httpContextAccessor;
    public LocalStorageService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _basePath = _configuration.GetValue<string>("Storage_Local:BasePath") ?? "";
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<string> UploadAsync(string containerName, string key, Stream content, string contentType, CancellationToken ct = default)
    {
        if (content is null)
            throw new BlobStorageException($"No content provided for upload: {key}");

        try
        {
            var containerPath = Path.Combine(_basePath, containerName);
            if (!Directory.Exists(containerPath))
                Directory.CreateDirectory(containerPath);

            var fullPath = Path.Combine(_basePath, containerName, key);

            var fileDirectory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(fileDirectory) && !Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);

            await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            await content.CopyToAsync(fileStream, ct);

            return key;
        }
        catch (Exception ex) when (ex is not BlobStorageException)
        {
            throw new BlobStorageException($"Local upload failed for {key} -  ${ex.Message}");
        }
    }
    public Task DeleteAsync(string containerName, string key, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_basePath, containerName,key); 
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        return Task.CompletedTask;
 
    }
    public async Task<Stream> DownloadAsync(string containerName, string key, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_basePath, containerName, key);
        if (!File.Exists(fullPath))
            throw new BlobStorageException($"File not found: {key}");

        var memoryStream = new MemoryStream();
        await using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
        {
            await fileStream.CopyToAsync(memoryStream, ct);
        }
        memoryStream.Position = 0;
        return memoryStream;
    }

    public Task<bool> ExistsAsync(string containerName, string key, CancellationToken ct = default)
    {
        var fullPath = Path.Combine(_basePath, containerName, key);
        return Task.FromResult(File.Exists(fullPath));
    }



    public Uri GetPublicUrl(string containerName, string key)
    {
        var cdnBase = _configuration.GetValue<string>("Storage_Local:BasePath");
        var baseUri = new Uri(GetRequestBaseUrl());
 
        return new Uri(baseUri, $"{cdnBase.TrimEnd('/')}/{containerName.TrimEnd('/')}/{key.TrimStart('/')}");
    }

    public Uri GetSignedUploadUrl(string containerName, string key, TimeSpan expiry)
    {
        throw new NotSupportedException(
            "Signed upload URLs are not supported by the local storage provider. Use direct proxy upload instead.");
    }

    private string GetRequestBaseUrl()
    {
        var request = _httpContextAccessor.HttpContext?.Request;
        if (request is null)
            throw new InvalidOperationException("No active HTTP request to build base URL from.");

        return $"{request.Scheme}://{request.Host}{request.PathBase}";
    }

}
