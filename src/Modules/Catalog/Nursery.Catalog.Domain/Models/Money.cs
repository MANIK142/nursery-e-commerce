
namespace Nursery.Catalog.Domain.Models;

public record Money
{
    public decimal Amount { get;  set; }
    public string Currency { get;  set; } = string.Empty!;
}

