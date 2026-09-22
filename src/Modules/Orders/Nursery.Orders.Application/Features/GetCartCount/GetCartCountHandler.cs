using BuildingBlocks.Common.CQRS;
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Features.GetCartCount;

public class GetCartCountHandler(IOrdersDbContext ordersDb) : IQueryHandler<GetCartCountQuery, GetCartCountResult>
{
    public async Task<GetCartCountResult> Handle(GetCartCountQuery request, CancellationToken cancellationToken)
    {
        int cart = await ordersDb.Carts.Include(c => c.Items)
                                        .Where(c=> c.CustomerId == request.CustomerId)
                                        .SelectMany(c => c.Items)
                                        .SumAsync(ci => ci.Quantity,cancellationToken);

        return new GetCartCountResult(cart);
    }
}
