using BuildingBlocks.Common.CQRS;
using MediatR;
using Nursery.Shippings.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Features.GetShipmentsByOrderId;

public sealed record GetShipmentsByOrderIdQuery(Guid OrderId) : IQuery<IReadOnlyList<ShipmentDto>>;
