

using Nursery.Shippings.Application.Data;

namespace Nursery.Shippings.Infrastucture.Presistance.Repository;


public class OrderLineItemLookup() : IOrderLineItemLookup
{
    public Task<IReadOnlyList<OrderLineItemDto>> GetLineItemsAsync(Guid orderId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
