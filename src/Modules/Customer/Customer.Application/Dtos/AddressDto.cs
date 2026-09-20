
using Customer.Domain.Enums;
using System.Text.Json.Serialization;
namespace Customer.Application.Dtos;

public record CustomerAddressDto(string ExternalUserId, [property: JsonConverter(typeof(JsonStringEnumConverter))] AddressType Type, string Line1, string? Line2,
        string City, string State, string PostalCode, string Country, bool IsDefault);

