
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Infrastructure.Exceptions;

namespace Nursery.Catalog.Infrastructure.Repository;

public class AzureBlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _client;
    private readonly IConfiguration _configuration;

    public AzureBlobStorageService(BlobServiceClient client,IConfiguration configuration)
    {
        _client = client;
        _configuration = configuration;
    }

    public async Task<string> UploadAsync(
        string containerName, string key, Stream content, string contentType, CancellationToken ct = default)
    {
        if (content is null)
            throw new BlobStorageException($"No content provided for upload: {key}");

        try
        {
            var container = await EnsureContainerExistsAsync(containerName, ct);
            var blob = container.GetBlobClient(key); // key already fully-formed by caller, e.g. "uploads/{guid}.jpg"

            await blob.UploadAsync(content, new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
            }, ct);

            return key;
        }
        catch (RequestFailedException ex)
        {
            throw new BlobStorageException($"Azure upload failed for {key}", ex);
        }
    }

    public async Task<Stream> DownloadAsync(string containerName, string key, CancellationToken ct = default)
    {
        try
        {
            var container = _client.GetBlobContainerClient(containerName);
            var blob = container.GetBlobClient(key);

            var response = await blob.DownloadStreamingAsync(cancellationToken: ct);

            var memoryStream = new MemoryStream();
            await response.Value.Content.CopyToAsync(memoryStream, ct);
            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            throw new BlobStorageException($"File not found: {key}", ex);
        }
        catch (RequestFailedException ex)
        {
            throw new BlobStorageException($"Azure download failed for {key}", ex);
        }
    }

    public async Task DeleteAsync(string containerName, string key, CancellationToken ct = default)
    {
        try
        {
            var container = _client.GetBlobContainerClient(containerName);
            await container.DeleteBlobIfExistsAsync(key, cancellationToken: ct);
        }
        catch (RequestFailedException ex)
        {
            throw new BlobStorageException($"Azure delete failed for {key}", ex);
        }
    }

    public async Task<bool> ExistsAsync(string containerName, string key, CancellationToken ct = default)
    {
        try
        {
            var container = _client.GetBlobContainerClient(containerName);
            var blob = container.GetBlobClient(key);
            var response = await blob.ExistsAsync(ct);
            return response.Value;
        }
        catch (RequestFailedException ex)
        {
            throw new BlobStorageException($"Azure existence check failed for {key}", ex);
        }
    }

    public Uri GetPublicUrl(string containerName, string key)
    {
        var cdnBase = _configuration.GetValue<string>("Storage_Azure:CdnBaseUrl");// _options.CdnBaseUrl?.TrimEnd('/');
        if (!string.IsNullOrEmpty(cdnBase))
            return new Uri($"{cdnBase}/{containerName}/{key}");

        // Fall back to the blob's own endpoint if no CDN configured
        var container = _client.GetBlobContainerClient(containerName);
        return container.GetBlobClient(key).Uri;
    }

    public Uri GetSignedUploadUrl(string containerName, string key, TimeSpan expiry)
    {
        var container = _client.GetBlobContainerClient(containerName);
        var blob = container.GetBlobClient(key);

        if (!blob.CanGenerateSasUri)
            throw new BlobStorageException(
                "Cannot generate SAS URL — ensure the BlobServiceClient was created with a connection string or shared key credential, not just a URI.");

        var sasBuilder = new Azure.Storage.Sas.BlobSasBuilder
        {
            BlobContainerName = containerName,
            BlobName = key,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(expiry)
        };
        sasBuilder.SetPermissions(Azure.Storage.Sas.BlobSasPermissions.Write | Azure.Storage.Sas.BlobSasPermissions.Create);

        return blob.GenerateSasUri(sasBuilder);
    }

    private async Task<BlobContainerClient> EnsureContainerExistsAsync(string containerName, CancellationToken ct)
    {
        var container = _client.GetBlobContainerClient(containerName);

        // PublicAccessType.Blob = anonymous read access to blobs, but not container listing —
        // equivalent to the public-read bucket policy we set on MinIO earlier.
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: ct);

        return container;
    }
}