using BuildingBlocks.Common.CQRS;
using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Application.Dto;
using Nursery.Orders.Application.Features.GetAllOrder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Features.GetAllOrder;

public class GetAllOrderHandler(IOrdersDbContext DbContext) : ICommandHandler<GetAllOrderQuery, GetAllOrderResult>
{
    public async Task<GetAllOrderResult?> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
    {

        var pageNumber = request.PageNumber ?? 0;
        var pageSize = request.PageSize ?? 100;

        var orders = await DbContext.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
            .OrderByDescending(c => c.CreatedAt)
            .Skip(pageNumber * pageSize)
            .Take(pageSize)
            .Select(o => new OrderDto
            {
                Id = o.Id,
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



        return new GetAllOrderResult(orders);
    }
}
