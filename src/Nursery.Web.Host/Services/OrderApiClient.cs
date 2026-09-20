using Nursery.Web.Host.Models.DTOs.Cart;
using Nursery.Web.Host.Models.DTOs.Catalog;
using Nursery.Web.Host.Services.Interface;

namespace Nursery.Web.Host.Services;

public record GetCartResult(CartDto Cart);
public class OrderApiClient : IOrderApiClient
{
    private readonly HttpClient _httpClient;
    public OrderApiClient(HttpClient httpClient) => _httpClient = httpClient;
    public async Task<CartDto> GetCartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/v1/carts", cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GetCartResult>(cancellationToken);
            return result.Cart;
        }
        catch (Exception ex)
        {
            return null;
            //_logger.LogError(ex, "Failed to reach Plant API.");
            //return (false, "An error occurred while communicating with the catalog service.");
        }
    }

    public async Task<(bool IsSuccess, string? ErrorMessage)> AddToCardAsync(Guid plantVariantid, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = new { plantVariantid };
            var response = await _httpClient.PostAsJsonAsync("/api/v1/carts/items", payload, cancellationToken);
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

    public async Task<(bool IsSuccess, string? ErrorMessage)> IncreaseCartItem(Guid plantVariantid, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsync($"/api/v1/carts/items/{plantVariantid}/increase", content: null , cancellationToken);
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

    public async Task<(bool IsSuccess, string? ErrorMessage)> DecreaseCartItem(Guid plantVariantid, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsync($"/api/v1/carts/items/{plantVariantid}/decrease", content: null, cancellationToken);
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

    public async Task<(bool IsSuccess, string? ErrorMessage)> DeleteCartItem(Guid plantVariantid, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/api/v1/carts/items/{plantVariantid}",  cancellationToken);
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
