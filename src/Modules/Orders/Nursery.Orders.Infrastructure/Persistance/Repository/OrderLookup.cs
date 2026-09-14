using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Infrastructure.Persistance.Context;
using Nursery.Payment.Api.Contracts;


namespace Nursery.Orders.Infrastructure.Persistance.Repository;

public class OrderLookup(OrdersDbContext ordersDb) : IOrderLookup
{
    public async Task<OrderPaymentInfo?> GetOrderPaymentInfoAsync(Guid orderId, CancellationToken ct)
    {
        var order = await ordersDb.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);
           
        if(order != null)
        {
            return new OrderPaymentInfo(order.Id, order.CustomerId, order.TotalOrderValue, order.Status.ToString());
        }
        return null;
       
    }
}
