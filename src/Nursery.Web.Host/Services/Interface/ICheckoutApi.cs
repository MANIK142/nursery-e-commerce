using Nursery.Web.Host.Models;
using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.DTOs.Cart;
using Nursery.Web.Host.Models.DTOs.Customer;

namespace Nursery.Web.Host.Services.Interface;

public interface ICheckoutApi
{
    Task<CartDto?> GetCartAsync(CancellationToken ct);
    Task<IReadOnlyList<AddressDto>> GetAddressesAsync(CancellationToken ct);
    Task<ApiResultModel<PlaceOrderResponse>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct);
}


