using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Dto;

public sealed record ShipmentLineItemDto(Guid OrderItemId, Guid ProductId, int Quantity);
