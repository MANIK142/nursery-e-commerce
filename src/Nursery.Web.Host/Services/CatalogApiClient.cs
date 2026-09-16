using Nursery.Web.Host.Models.Catalog;
using System.Net;

namespace Nursery.Web.Host.Services;


public class CatalogApiClient : ICatalogApiClient
{
    private readonly HttpClient _httpClient;

    public CatalogApiClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<IReadOnlyList<PlantDto>> GetPlantsAsync(CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync("api/v1/plants", ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<PlantsResponse>(cancellationToken: ct);
        return result?.Plants ?? [];
    }

    public async Task<PlantDto?> GetPlantByIdAsync(Guid id, CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync($"api/v1/plants?id={id}", ct);
        if (response.StatusCode == HttpStatusCode.NotFound) return null;


        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<PlantsResponse>(cancellationToken: ct);
        return result?.Plants.FirstOrDefault() ?? null;
    }
}