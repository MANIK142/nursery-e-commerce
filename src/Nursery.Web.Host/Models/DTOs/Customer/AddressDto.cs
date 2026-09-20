namespace Nursery.Web.Host.Models.DTOs.Customer;

public sealed class AddressDto
{
    public Guid ExternalUserId { get; set; }
    public string? Type { get; set; }
    public string Line1 { get; set; } = "";
    public string? Line2 { get; set; }
    public string City { get; set; } = "";
    public string State { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string country { get; set; } = "";
    public bool IsDefault { get; set; }
}
