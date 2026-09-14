using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Dto;

public sealed record ShipmentDto(
 Guid Id,
 Guid OrderId,
 string Status,
 string? Carrier,
 string? TrackingNumber,
 string? TrackingUrl,
 DateTimeOffset CreatedAt,
 DateTimeOffset? ShippedAt,
 DateTimeOffset? DeliveredAt,
 IReadOnlyList<ShipmentLineItemDto> LineItems);
