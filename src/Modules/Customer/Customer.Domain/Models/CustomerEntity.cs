using BuildingBlocks.Common;
using Customer.Domain.Enums;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Emit;
namespace Customer.Domain.Models;
public class CustomerEntity : BaseDomainModel
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string ExternalUserId { get; private set; } = default!;
    public string EmailAddress { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    private readonly List<Address> _addresses = new();
    public IReadOnlyCollection<Address> Addresses => _addresses.AsReadOnly();

    private CustomerEntity() { }

    private CustomerEntity(Guid id,string firstName, string lastName, string externalUserId, string emailAddress, string phoneNumber)
    {
        Id = id;
        ExternalUserId = externalUserId;
        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        PhoneNumber = phoneNumber;
        SetCreated(Id.ToString());
    }
    public static CustomerEntity Create(string firstName,string lastName,string externalUserId,string emailAddress,string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("Customer name is required.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(externalUserId))
            throw new ArgumentException("Indentity Id is required.", nameof(externalUserId));

        var customer = new CustomerEntity(Guid.NewGuid(),firstName,lastName,externalUserId,emailAddress,phoneNumber);
        return customer;
    }

    public void UpdateProfile(string firstName, string lastName, string? phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First Name is required.", nameof(firstName));
        if(string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last Name Id is required.", nameof(lastName));

        if (FirstName == firstName && LastName == lastName && PhoneNumber == phoneNumber)
            return; 

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }

    public void ChangeEmail(string newEmail)
    {
        if (string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentException("Email Address is required.", nameof(newEmail));
        if (EmailAddress == newEmail) return;
        EmailAddress = newEmail;
    }


    public Address AddAddress(AddressType type, string line1, string? line2, string city,
        string state, string postalCode, string country, bool isDefault = false)
    {
        var address = Address.Create(Id, type, line1, line2, city, state, postalCode, country, isDefault);
        if (isDefault)
            ClearDefaultFor(type);

        _addresses.Add(address);
        return address;
    }

    public void SetDefaultAddress(Guid addressId)
    {
        var target = _addresses.SingleOrDefault(a => a.Id == addressId);
        if (target == null)
            throw new ArgumentException("Address not found", nameof(addressId));

        ClearDefaultFor(target!.Type);
        target.MarkAsDefault();
    }

    public void RemoveAddress(Guid addressId)
    {
        var target = _addresses.SingleOrDefault(a => a.Id == addressId);
        if (target == null)
            throw new ArgumentException("Address not found", nameof(addressId));
        _addresses.Remove(target!);
    }

    private void ClearDefaultFor(AddressType type)
    {
        foreach (var addr in _addresses.Where(a => a.Type == type && a.IsDefault))
            addr.UnmarkAsDefault();
    }

}
