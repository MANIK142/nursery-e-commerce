using BuildingBlocks.Common.CQRS;
using Nursery.Orders.Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Features.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId, Guid CustomerId) : ICommand<GetOrderByIdResult>;
public record GetOrderByIdResult(OrderDto Order);