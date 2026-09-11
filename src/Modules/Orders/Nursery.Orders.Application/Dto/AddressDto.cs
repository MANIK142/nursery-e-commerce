
namespace Nursery.Orders.Application.Dto;

public record AddressDto(
    string FirstName,
    string LastName,
    string? EmailAddress,
    string AddressLine,
    string Country,
    string State,
    string ZipCode
    );