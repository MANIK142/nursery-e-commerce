
namespace Nursery.Catalog.Application.Data;
public interface IBlobStorageService
{
    Task<string> UploadAsync(
    string containerName,
    string key,
    Stream content,
    string contentType,
    CancellationToken ct = default);

    Task<Stream> DownloadAsync(string containerName, string key, CancellationToken ct = default);

    Task DeleteAsync(string containerName, string key, CancellationToken ct = default);

    Task<bool> ExistsAsync(string containerName, string key, CancellationToken ct = default);

    Uri GetPublicUrl(string containerName, string key);

    Uri GetSignedUploadUrl(string containerName, string key, TimeSpan expiry);
}
