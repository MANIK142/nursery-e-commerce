
using Nursery.Orders.Domain.Carts;
using Nursery.Orders.Domain.Orders;

namespace Nursery.Orders.Application.Data;
public interface IOrderRepository
{
    Task<List<Order>?> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken ct);
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken ct);
    Task AddOrderAsync(Order order,CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken ct);
}
