using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Nursery.Payment.Api.Contracts;
using Nursery.Payment.Api.Models;

namespace Nursery.Payment.Api.Persistance.Repository;

public class ProcessedWebhookEventRepository(PaymentDbContext context) : IProcessedWebhookEventRepository
{
    private const int UniqueConstraintViolation = 2627;
    private const int UniqueIndexViolation = 2601;

    public async Task<bool> ExistsAsync(string eventId, CancellationToken ct)
        => await context.ProcessedWebhookEvents.AsNoTracking().AnyAsync(e => e.EventId == eventId, ct);

    public async Task MarkProcessedAsync(string eventId, CancellationToken ct)
    {
        context.ProcessedWebhookEvents.Add(new ProcessedWebhookEvent(eventId));
        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (IsDuplicateKeyViolation(ex))
        {
            // Two concurrent webhook deliveries for the same event raced past
            // ExistsAsync before either committed. The event is already
            // recorded — this is a successful no-op, not a failure.
        }
    }
    private static bool IsDuplicateKeyViolation(DbUpdateException ex)
       => ex.InnerException is SqlException sqlEx
          && (sqlEx.Number == UniqueConstraintViolation || sqlEx.Number == UniqueIndexViolation);
}
