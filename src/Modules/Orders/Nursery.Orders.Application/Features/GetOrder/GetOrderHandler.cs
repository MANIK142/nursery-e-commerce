using BuildingBlocks.Common.CQRS;
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Features.GetOrder;

public class GetOrderHandler(IOrdersDbContext DbContext) : ICommandHandler<GetOrderQuery, GetOrderResult>
{
    public async Task<GetOrderResult?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var orders = await DbContext.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == request.CustomerId)
            .Include(o => o.OrderItems)
            .Select(o => new OrderDto
            {
                CustomerId = o.CustomerId,
                PaymentStatus = o.PaymentStatus,
                Status = o.Status,
                OrderItems = o.OrderItems.Select(
                                                    oi => new OrderItemDto
                                                    {
                                                        Id = oi.Id,
                                                        OrderId = oi.OrderId,
                                                        PlantVariantId = oi.PlantVariantId,
                                                        ProductNameAtPurchase = oi.ProductNameAtPurchase,
                                                        UnitPriceAtPurchase = oi.UnitPriceAtPurchase,
                                                        Quantity = oi.Quantity
                                                    }
                                                ).ToList()
            }).ToListAsync(cancellationToken);



        return new GetOrderResult(orders);
    }
}
