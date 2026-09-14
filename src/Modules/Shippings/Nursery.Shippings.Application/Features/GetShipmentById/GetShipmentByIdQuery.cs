using BuildingBlocks.Common.CQRS;
using MediatR;
using Nursery.Shippings.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Features.GetShipmentById;

public sealed record GetShipmentByIdQuery(Guid ShipmentId) : IQuery<ShipmentDto?>;

