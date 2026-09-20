namespace Nursery.Web.Host.Models.DTOs.Cart;

public record CartDto(Guid Id, Guid CustomerId, decimal TotalPrice, List<CartItemDto> CartItemDtos);