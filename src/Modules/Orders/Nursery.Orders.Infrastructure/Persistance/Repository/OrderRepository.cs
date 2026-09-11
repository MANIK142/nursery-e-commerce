
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Domain.Orders;
using Nursery.Orders.Infrastructure.Persistance.Context;

namespace Nursery.Orders.Infrastructure.Persistance.Repository;

public class OrderRepository(OrdersDbContext ordersDb) : IOrderRepository
{
    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await ordersDb.AddAsync(order, cancellationToken);
    }

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken ct)
    {
        return await ordersDb.Orders.Include(o=> o.OrderItems).FirstOrDefaultAsync(o =>o.Id == orderId, ct);
    }

    public async Task<List<Order>?> GetOrdersByCustomerIdAsync(Guid customerId, CancellationToken ct)
    {
        return await ordersDb.Orders.Include(o => o.OrderItems).Where(o => o.CustomerId == customerId).ToListAsync(ct);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken ct)
    {
        var rowafftected = await ordersDb.SaveChangesAsync(ct);
        return rowafftected > 0;
    }
}
