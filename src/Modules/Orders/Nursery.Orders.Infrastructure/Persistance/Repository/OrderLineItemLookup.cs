

using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using Nursery.Shippings.Application.Data;

namespace Nursery.Orders.Infrastructure.Persistance.Repository;

public class OrderLineItemLookup(IOrdersDbContext dbContext) : IOrderLineItemLookup
{
    public async Task<IReadOnlyList<OrderLineItemDto>> GetLineItemsAsync(Guid orderId, CancellationToken ct)
    {
        return await dbContext.OrderItems
        .AsNoTracking()
        .Where(li => li.OrderId == orderId)
        .Select(li => new OrderLineItemDto(li.Id, li.PlantVariantId, li.Quantity))
        .ToListAsync(ct);
    }
}
