using BuildingBlocks.Common.CQRS;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Application.Dto;
using Nursery.Orders.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Features.GetOrderById;

public class GetOrderByIdHandler(IOrderRepository repository) : ICommandHandler<GetOrderByIdQuery, GetOrderByIdResult>
{
    public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.OrderId, cancellationToken)
                          ?? throw new OrderNotFoundException($"Order not found for this Order id {request.OrderId}");

        List<OrderItemDto> OrderItems = [];
        foreach (var item in order.OrderItems) {
                var orderItemDto = new OrderItemDto
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    PlantVariantId = item.PlantVariantId,
                    ProductNameAtPurchase = item.ProductNameAtPurchase,
                    Quantity = item.Quantity,
                    UnitPriceAtPurchase= item.UnitPriceAtPurchase,
                };
            OrderItems.Add(orderItemDto);
        }
        var orderDto = new OrderDto
        {
            CustomerId = order.CustomerId,
            PaymentStatus = order.PaymentStatus,
            Status = order.Status,
            OrderItems = OrderItems
        };

      return new GetOrderByIdResult(orderDto);
    }
}
