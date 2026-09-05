
namespace Nursery.Catalog.Domain.Models;

public record Money
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty!;
    private Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
    public static Money Create(decimal amount, string currency = "Rs.")
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount Should be Greater than 0!");
        }
        return new Money(amount, currency);
    }
}

