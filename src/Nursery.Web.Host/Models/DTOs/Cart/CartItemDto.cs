namespace Nursery.Web.Host.Models.DTOs.Cart;

public record CartItemDto(Guid PlantVariantId, int Quantity, decimal Price);