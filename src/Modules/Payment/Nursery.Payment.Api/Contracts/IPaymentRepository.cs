namespace Nursery.Payment.Api.Contracts;

public interface IPaymentRepository
{
    Task<Models.Payment?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Models.Payment?> GetByOrderIdAsync(Guid orderId, CancellationToken ct);
    Task<Models.Payment?> GetByGatewayPaymentIntentIdAsync(string gatewayPaymentIntentId, CancellationToken ct);
    Task AddAsync(Models.Payment payment, CancellationToken ct);
    Task UpdateAsync(Models.Payment payment, CancellationToken ct);
}
