using BuildingBlocks.Common;
using Customer.Domain.Enums;
namespace Customer.Domain.Models;

public class Address : BaseDomainModel
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; private set; }
    public AddressType Type { get; private set; }
    public string Line1 { get; private set; } = default!;
    public string? Line2 { get; private set; }
    public string City { get; private set; } = default!;
    public string State { get; private set; } = default!;
    public string PostalCode { get; private set; } = default!;
    public string Country { get; private set; } = default!;
    public bool IsDefault { get; private set; }

    private Address() { } // EF Core

    private Address(Guid id, Guid customerId, AddressType type, string line1, string? line2,
        string city, string state, string postalCode, string country, bool isDefault)
    {
        Id = id;
        CustomerId = customerId;
        Type = type;
        Line1 = line1;
        Line2 = line2;
        City = city;
        State = state;
        PostalCode = postalCode;
        Country = country;
        IsDefault = isDefault;
        SetCreated(customerId.ToString());
    }

    internal static Address Create(Guid customerId, AddressType type, string line1, string? line2,
        string city, string state, string postalCode, string country, bool isDefault)
    {
        if (string.IsNullOrWhiteSpace(line1))
            throw new ArgumentException("Line1 is required.", nameof(line1));
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(state))
            throw new ArgumentException("State  is required.", nameof(state));
        if (string.IsNullOrWhiteSpace(postalCode))
            throw new ArgumentException("Postal Code  is required.", nameof(postalCode));
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentException("Country is required.", nameof(country));


        return new Address(Guid.NewGuid(), customerId, type, line1, line2, city, state, postalCode, country, isDefault);
    }

    internal void MarkAsDefault() => IsDefault = true;
    internal void UnmarkAsDefault() => IsDefault = false;
}
