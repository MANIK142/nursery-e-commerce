using Nursery.Web.Host.Models;
using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.DTOs.Cart;
using Nursery.Web.Host.Models.DTOs.Customer;
using Nursery.Web.Host.Services.Interface;

namespace Nursery.Web.Host.Services;

public class CheckoutApi(ICatalogApiClient catalog,IOrderApiClient order, ICheckoutApi checkoutApi ) : ICheckoutApi
{
    public Task<IReadOnlyList<AddressDto>> GetAddressesAsync(CancellationToken ct)
    {
        return checkoutApi.GetAddressesAsync(ct);
    }

    public Task<CartDto?> GetCartAsync(CancellationToken ct)
    {
        return order.GetCartAsync(ct)
    }

    public Task<ApiResultModel<PlaceOrderResponse>> PlaceOrderAsync(PlaceOrderRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
