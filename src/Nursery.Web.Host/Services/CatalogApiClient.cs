using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.DTOs.Catalog;
using Nursery.Web.Host.Services.Interface;
using System.Net;

namespace Nursery.Web.Host.Services;


public class CatalogApiClient : ICatalogApiClient
{
    private readonly HttpClient _httpClient;

    public CatalogApiClient(HttpClient httpClient) => _httpClient = httpClient;



    public async Task<(bool IsSuccess, string? ErrorMessage)> CreatePlantAsync(
        CreatePlantApiRequest payload,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/plants", payload, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var rawError = await response.Content.ReadAsStringAsync(cancellationToken);
            //_logger.LogWarning("Plant creation failed with status {StatusCode}: {Response}", response.StatusCode, rawError);
            return (false, string.IsNullOrWhiteSpace(rawError) ? "API rejected the plant submission." : rawError);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Failed to reach Plant API.");
            return (false, "An error occurred while communicating with the catalog service.");
        }
    }

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

    public async Task<IEnumerable<CategoryDto>?> GetCatgoriesAsync(CancellationToken ct = default)
    {
        var response = await _httpClient.GetAsync("/api/v1/category", ct);
        if (response.StatusCode == HttpStatusCode.NotFound) return null;


        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<CatagoryResponse>(cancellationToken: ct);

        return result?.Categories ?? [];
    }

    public async Task<(bool IsSuccess, string? ErrorMessage)> UpdatePlantAsync(
     Guid id,
     UpdatePlantApiRequest payload,
     CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/v1/plants", payload, cancellationToken);
            if (response.IsSuccessStatusCode) return (true, null);

            var rawError = await response.Content.ReadAsStringAsync(cancellationToken);
            return (false, string.IsNullOrWhiteSpace(rawError) ? "Failed to update plant." : rawError);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Failed to send update for plant {PlantId}", id);
            return (false, "Communication error with catalog service.");
        }
    }

    public async Task<(bool IsSuccess, string? ErrorMessage)> CreateCareInstruction(
                                                                                 CreateCareInstructionRequest payload,
                                                                                 CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/v1/careinstruction", payload, cancellationToken);
            if (response.IsSuccessStatusCode) return (true, null);

            var rawError = await response.Content.ReadAsStringAsync(cancellationToken);
            return (false, string.IsNullOrWhiteSpace(rawError) ? "Failed to update plant." : rawError);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Failed to send update for plant {PlantId}", id);
            return (false, "Communication error with catalog service.");
        }
    }

    public async Task<(bool IsSuccess, string? ErrorMessage)> UpdateCareInstruction(
                                                                              UpdateCareInstructionRequest payload,
                                                                              CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/v1/careinstruction", payload, cancellationToken);
            if (response.IsSuccessStatusCode) return (true, null);

            var rawError = await response.Content.ReadAsStringAsync(cancellationToken);
            return (false, string.IsNullOrWhiteSpace(rawError) ? "Failed to update plant." : rawError);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Failed to send update for plant {PlantId}", id);
            return (false, "Communication error with catalog service.");
        }
    }


    public async Task<(bool IsSuccess, string? ErrorMessage)> DeletePlant(Guid PlantId, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/plants/{PlantId}",cancellationToken);
            if (response.IsSuccessStatusCode) return (true, null);

            var rawError = await response.Content.ReadAsStringAsync(cancellationToken);
            return (false, string.IsNullOrWhiteSpace(rawError) ? "Failed to update plant." : rawError);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Failed to send update for plant {PlantId}", id);
            return (false, "Communication error with catalog service.");
        }
    }



    


}