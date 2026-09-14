using Microsoft.EntityFrameworkCore;
using Nursery.Payment.Api.Contracts;

namespace Nursery.Payment.Api.Persistance.Repository;

public class PaymentRepository(PaymentDbContext context) : IPaymentRepository
{
    public async Task<Models.Payment?> GetByIdAsync(Guid id, CancellationToken ct)
         => await context.Payments
             .Include(p => p.Attempts)
             .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Models.Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken ct)
        => await context.Payments
            .Include(p => p.Attempts)
            .FirstOrDefaultAsync(p => p.OrderId == orderId, ct);

    public async Task<Models.Payment?> GetByGatewayPaymentIntentIdAsync(string gatewayPaymentIntentId, CancellationToken ct)
        => await context.Payments
            .Include(p => p.Attempts)
            .FirstOrDefaultAsync(
                p => p.Attempts.Any(a => a.GatewayPaymentIntentId == gatewayPaymentIntentId), ct);

    public async Task AddAsync(Models.Payment payment, CancellationToken ct)
    {
        await context.Payments.AddAsync(payment, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Models.Payment payment, CancellationToken ct)
    {
        await context.SaveChangesAsync(ct);
    }
}
