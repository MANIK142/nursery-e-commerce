using BuildingBlocks.Common.CQRS;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Features.GetShipmentById;

public sealed class GetShipmentByIdQueryHandler(IShippingDbContext dbContext): IQueryHandler<GetShipmentByIdQuery, ShipmentDto?>
{
    public async Task<ShipmentDto?> Handle(GetShipmentByIdQuery request, CancellationToken ct)
    {
        return await dbContext.Shipments
            .AsNoTracking()
            .Where(s => s.Id == request.ShipmentId)
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
            .FirstOrDefaultAsync(ct);
    }
}
