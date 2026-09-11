
using Nursery.Orders.Domain.Carts;

namespace Nursery.Orders.Application.Data;
public interface ICartRepository
{
    Task<Cart?> GetActiveCartByCustomerIdAsync(Guid customerId, CancellationToken ct);
    Task<Cart?> GetByIdAsync(Guid cartId, CancellationToken ct);
    Task AddAsync(Cart cart, CancellationToken ct);
    Task<bool> SaveChangesAsync(CancellationToken ct);
}
