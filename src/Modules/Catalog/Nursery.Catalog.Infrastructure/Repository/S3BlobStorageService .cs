

using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Infrastructure.Exceptions;

namespace Nursery.Catalog.Infrastructure.Repository;

public class S3BlobStorageService : IBlobStorageService
{
    private readonly IAmazonS3 _client;
    private readonly IConfiguration _configuration;

    public S3BlobStorageService(IAmazonS3 client, IConfiguration configuration)
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
            await EnsureBucketExistsAsync(containerName, ct);

            var request = new PutObjectRequest
            {
                BucketName = containerName,
                Key = key,               // already fully-formed by the caller (e.g. "uploads/{guid}.jpg")
                InputStream = content,
                ContentType = contentType,
                AutoCloseStream = false, // caller owns the stream lifecycle
                ChecksumAlgorithm = null
            };

            await _client.PutObjectAsync(request, ct);
            return key;
        }
        catch (AmazonS3Exception ex)
        {
            throw new BlobStorageException($"S3 upload failed for {key}", ex);
        }
    }

    public async Task<Stream> DownloadAsync(string containerName, string key, CancellationToken ct = default)
    {
        try
        {
            var response = await _client.GetObjectAsync(containerName, key, ct);

            // Copy into a MemoryStream so the caller isn't tied to the S3 response's
            // underlying connection lifetime.
            var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream, ct);
            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            throw new BlobStorageException($"File not found: {key}", ex);
        }
        catch (AmazonS3Exception ex)
        {
            throw new BlobStorageException($"S3 download failed for {key}", ex);
        }
    }

    public async Task DeleteAsync(string containerName, string key, CancellationToken ct = default)
    {
        try
        {
            await _client.DeleteObjectAsync(containerName, key, ct);
        }
        catch (AmazonS3Exception ex)
        {
            throw new BlobStorageException($"S3 delete failed for {key}", ex);
        }
    }

    public async Task<bool> ExistsAsync(string containerName, string key, CancellationToken ct = default)
    {
        try
        {
            await _client.GetObjectMetadataAsync(containerName, key, ct);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        catch (AmazonS3Exception ex)
        {
            throw new BlobStorageException($"S3 existence check failed for {key}", ex);
        }
    }

    public Uri GetPublicUrl(string containerName, string key)
    {
        var serviceUrl = _configuration.GetValue<string>("Storage_Aws:S3:ServiceUrl");
        var cdnBase = serviceUrl?.TrimEnd('/');
        return string.IsNullOrEmpty(cdnBase)
            ? new Uri($"{serviceUrl.TrimEnd('/')}/{containerName}/{key}")
            : new Uri($"{cdnBase}/{containerName}/{key}");
    }

    public Uri GetSignedUploadUrl(string containerName, string key, TimeSpan expiry)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = containerName,
            Key = key,
            Verb = HttpVerb.PUT,
            Expires = DateTime.UtcNow.Add(expiry)
        };
        return new Uri(_client.GetPreSignedURL(request));
    }

    private async Task EnsureBucketExistsAsync(string bucket, CancellationToken ct)
    {
        try
        {
            await _client.PutBucketAsync(bucket, ct);
            await ApplyPublicReadPolicyAsync(bucket, ct);
        }
        catch (AmazonS3Exception ex) when (
            ex.ErrorCode == "BucketAlreadyOwnedByYou" ||
            ex.ErrorCode == "BucketAlreadyExists")
        {
            // Bucket already exists — that's fine, nothing to do.
        }
    }
    private async Task ApplyPublicReadPolicyAsync(string bucket, CancellationToken ct)
    {
        var policy = $$"""
                    {
                      "Version": "2012-10-17",
                      "Statement": [
                        {
                          "Effect": "Allow",
                          "Principal": "*",
                          "Action": "s3:GetObject",
                          "Resource": "arn:aws:s3:::{{bucket}}/*"
                        }
                      ]
                    }
                    """;

        await _client.PutBucketPolicyAsync(bucket, policy, ct);
    }
}
