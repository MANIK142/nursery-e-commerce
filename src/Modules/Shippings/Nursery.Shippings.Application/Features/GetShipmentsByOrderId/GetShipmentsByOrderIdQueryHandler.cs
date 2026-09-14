using BuildingBlocks.Common.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Features.GetShipmentsByOrderId;

public sealed class GetShipmentsByOrderIdQueryHandler(IShippingDbContext dbContext)
: IQueryHandler<GetShipmentsByOrderIdQuery, IReadOnlyList<ShipmentDto>>
{
    public async Task<IReadOnlyList<ShipmentDto>> Handle(GetShipmentsByOrderIdQuery request, CancellationToken ct)
    {
        return await dbContext.Shipments.AsNoTracking()
            .Where(s => s.OrderId == request.OrderId)
            .Select(s => new ShipmentDto(
                s.Id,
                s.OrderId,
                s.Status.ToString(),
                s.TrackingInfo!.Carrier,
                s.TrackingInfo.TrackingNumber,
                s.TrackingInfo.TrackingUrl,
                s.CreatedAt,
                s.ShippedAt,
                s.DeliveredAt,
                s.LineItems.Select(li => new ShipmentLineItemDto(li.OrderItemId, li.ProductId, li.Quantity)).ToList()))
            .ToListAsync(ct);
    }
}
