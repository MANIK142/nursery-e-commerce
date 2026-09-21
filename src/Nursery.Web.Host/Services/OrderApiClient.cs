using Microsoft.AspNetCore.Identity.Data;
using Nursery.Web.Host.Models;
using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.DTOs.Cart;
using Nursery.Web.Host.Models.DTOs.Catalog;
using Nursery.Web.Host.Models.DTOs.Orders;
using Nursery.Web.Host.Services.Interface;
using System.Net;
using static System.Net.WebRequestMethods;

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

    public record CreateOrderApiResponse(Guid OrderId);
    public async Task<CreateOrderApiResponse> CreateOrder(CreateOrderApiRequest payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("/api/v1/orders", payload, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<CreateOrderApiResponse>(cancellationToken);
            return result;
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Failed to send update for plant {PlantId}", id);
            return null;
        }
    }
    public record GetOrderIdResposne(OrderDto order);
    public async Task<OrderDto?> GetOrderAsync(Guid orderId, CancellationToken ct)
    {
        using var res = await _httpClient.GetAsync($"api/v1/orders/{orderId}", ct);

        // The API scopes GetOrderById to the caller's CustomerId, so someone else's order looks like "not found".
        if (res.StatusCode is HttpStatusCode.NotFound or HttpStatusCode.Forbidden) return null;

        res.EnsureSuccessStatusCode();
        var body = await res.Content.ReadFromJsonAsync<GetOrderIdResposne>(ct);
        return body.order;
    }

    public async Task<ApiResultModel<InitiatePaymentResponse>> InitiatePaymentAsync(Guid orderId, CancellationToken ct)
    {
        using var res = await _httpClient.PostAsJsonAsync("api/v1/payments/initiate", new { orderId }, ct);

        if (!res.IsSuccessStatusCode)
            return ApiResultModel<InitiatePaymentResponse>.Failed("We couldn't start your payment. Please try again.");

        var body = await res.Content.ReadFromJsonAsync<InitiatePaymentResponse>(ct);
        return string.IsNullOrWhiteSpace(body?.ClientSecret)
            ? ApiResultModel<InitiatePaymentResponse>.Failed("We couldn't start your payment. Please try again.")
            : ApiResultModel<InitiatePaymentResponse>.Succeeded(body);
    }
}
