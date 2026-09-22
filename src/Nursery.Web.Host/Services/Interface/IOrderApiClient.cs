using Nursery.Web.Host.Models;
using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.DTOs.Cart;
using Nursery.Web.Host.Models.DTOs.Orders;
using static Nursery.Web.Host.Services.OrderApiClient;

namespace Nursery.Web.Host.Services.Interface;

public interface IOrderApiClient
{
    Task<(bool IsSuccess, string? ErrorMessage)> AddToCardAsync(Guid plantVariantid, CancellationToken cancellationToken = default);
    Task<CartDto> GetCartAsync(CancellationToken cancellationToken);
    Task<int> GetCartCount(CancellationToken cancellationToken);


    Task<(bool IsSuccess, string? ErrorMessage)> IncreaseCartItem(Guid plantVariantid, CancellationToken cancellationToken = default);

    Task<(bool IsSuccess, string? ErrorMessage)> DecreaseCartItem(Guid plantVariantid, CancellationToken cancellationToken = default);

    Task<(bool IsSuccess, string? ErrorMessage)> DeleteCartItem(Guid plantVariantid, CancellationToken cancellationToken = default);

    Task<CreateOrderApiResponse> CreateOrder(CreateOrderApiRequest payload, CancellationToken cancellationToken = default);

    Task<List<OrderDto>?> GetAllOrderAsync(CancellationToken ct);

    Task<OrderDto?> GetOrderAsync(Guid orderId, CancellationToken ct);

    Task<ApiResultModel<InitiatePaymentResponse>> InitiatePaymentAsync(Guid orderId, CancellationToken ct);

}
