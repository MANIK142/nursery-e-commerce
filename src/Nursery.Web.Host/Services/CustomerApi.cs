using Nursery.Web.Host.Models.DTOs.Catalog;
using Nursery.Web.Host.Models.DTOs.Customer;
using Nursery.Web.Host.Services.Interface;
using System.Net;

namespace Nursery.Web.Host.Services;

public class CustomerApi(HttpClient httpClient,ICurrentUserService currentUser) : ICustomerApi
{
    private readonly HttpClient _httpClient = httpClient;

    public Task<List<AddressDto>> GetCustomerAddresses(CancellationToken cancellation)
    {
        var customerId = currentUser.CustomerId;
        var response = await _httpClient.GetAsync($"/api/v1/Customer/GetAddresses/{customerId}", ct);
        if (response.StatusCode == HttpStatusCode.NotFound) return null;
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<{ List<AddressDto customerAddresses }>>(cancellationToken: ct);

        return result?.customerAddresses ?? [];
    }
}
