
using Customer.Domain.Enums;
namespace Customer.Application.Dtos;

public record AddressDto(string ExternalUserId, AddressType Type, string Line1, string? Line2,
        string City, string State, string PostalCode, string Country, bool IsDefault);

