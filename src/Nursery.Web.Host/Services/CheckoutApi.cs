using Nursery.Web.Host.Models;
using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.DTOs.Cart;
using Nursery.Web.Host.Models.DTOs.Catalog;
using Nursery.Web.Host.Models.DTOs.Customer;
using Nursery.Web.Host.Models.ViewModels.Cart;
using Nursery.Web.Host.Models.ViewModels.Checkout;
using Nursery.Web.Host.Services.Interface;

namespace Nursery.Web.Host.Services;

public record GetAddressesResponse(IReadOnlyList<AddressDto> CustomerAddresses);
public class CheckoutApi(HttpClient httpClient,ICurrentUserService currentUserService ) : ICheckoutApi
{
    private readonly HttpClient _httpClient = httpClient;

    public ICurrentUserService CurrentUserService { get; } = currentUserService;

    public async Task<IReadOnlyList<AddressDto>> GetAddressesAsync(CancellationToken ct)
    {
        var customerId = CurrentUserService.CustomerId;
        var response = await _httpClient.GetAsync($"/api/v1/Customer/GetAddresses/{customerId}", ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GetAddressesResponse>(cancellationToken: ct);
        return result?.CustomerAddresses ?? [];
    }



    public Task<ApiResultModel<PlaceOrderResponse>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
