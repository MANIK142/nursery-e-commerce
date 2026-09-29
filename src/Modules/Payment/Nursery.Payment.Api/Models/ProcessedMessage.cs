namespace Nursery.Payment.Api.Models;

public class ProcessedMessage
{
    public Guid MessageId { get; set; }
    public DateTime ProcessedAtUtc { get; set; }
}
