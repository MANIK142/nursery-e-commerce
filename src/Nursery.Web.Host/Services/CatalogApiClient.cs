using Nursery.Web.Host.Models.Catalog;
using System.Net;

namespace Nursery.Web.Host.Services;


public class CatalogApiClient : ICatalogApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatalogApiClient> _logger;

    public CatalogApiClient(HttpClient httpClient, ILogger<CatalogApiClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<PagedResult<PlantSummaryDto>> GetPlantsAsync(int page = 0, int pageSize = 3, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"api/v1/plants?PageNumber={page}&PageSize={pageSize}", ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PagedResult<PlantSummaryDto>>(cancellationToken: ct);
        return result ?? new PagedResult<PlantSummaryDto>([], page, pageSize, 0);
    }

    public async Task<PlantDetailDto?> GetPlantByIdAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"api/v1/plants/{id}", ct);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<PlantDetailDto>(cancellationToken: ct);
    }
}