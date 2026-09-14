using Microsoft.EntityFrameworkCore;
using Nursery.Payment.Api.Models;
namespace Nursery.Payment.Api.Data.Interface;

public interface IPaymentbContext
{
    DbSet<Nursery.Payment.Api.Models.Payment> Payments { get; }
    DbSet<PaymentAttempt> PaymentAttempts { get; }
    DbSet<ProcessedWebhookEvent> ProcessedWebhookEvents { get; }
}
