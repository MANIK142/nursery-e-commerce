using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Data;

public interface IOrderLineItemLookup
{
    Task<IReadOnlyList<OrderLineItemDto>> GetLineItemsAsync(Guid orderId, CancellationToken ct);
}
public sealed record OrderLineItemDto(Guid OrderItemId, Guid ProductId, int Quantity);

