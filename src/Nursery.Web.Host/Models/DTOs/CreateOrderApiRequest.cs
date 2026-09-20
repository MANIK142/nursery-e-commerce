namespace Nursery.Web.Host.Models.DTOs;

public record CreateOrderApiRequest(
    string CustomerId,
    OrderAddressDto BillingAddress,
    OrderAddressDto ShippingAddress,
    List<OrderItemDto> Items
);

public record OrderAddressDto(
    string FirstName,
    string LastName,
    string? EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode
);

public record OrderItemDto(
    Guid PlantVariantId, 
    int Quantity
);