using Nursery.Web.Host.Models.DTOs.Cart;

namespace Nursery.Web.Host.Services.Interface;

public interface IOrderApiClient
{
    Task<(bool IsSuccess, string? ErrorMessage)> AddToCardAsync(Guid plantVariantid, CancellationToken cancellationToken = default);
    Task<CartDto> GetCartAsync(CancellationToken cancellationToken);

    Task<(bool IsSuccess, string? ErrorMessage)> IncreaseCartItem(Guid plantVariantid, CancellationToken cancellationToken = default);

    Task<(bool IsSuccess, string? ErrorMessage)> DecreaseCartItem(Guid plantVariantid, CancellationToken cancellationToken = default);

    Task<(bool IsSuccess, string? ErrorMessage)> DeleteCartItem(Guid plantVariantid, CancellationToken cancellationToken = default);
}
