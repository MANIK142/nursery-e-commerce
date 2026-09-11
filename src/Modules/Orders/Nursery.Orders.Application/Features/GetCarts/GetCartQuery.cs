using BuildingBlocks.Common.CQRS;
using Nursery.Orders.Application.Dto;
using Nursery.Orders.Domain.Carts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Features.GetCarts;

public record GetCartQuery(Guid CustomerId) : IQuery<GetCartResult>;

public record GetCartResult(CartDto Cart);