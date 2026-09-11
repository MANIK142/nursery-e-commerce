
using BuildingBlocks.Common.CQRS;
using Nursery.Orders.Application.Dto;
using System.Windows.Input;

namespace Nursery.Orders.Application.Features.GetOrder;

public record GetOrderQuery(Guid CustomerId) : ICommand<GetOrderResult>;

public record GetOrderResult(IEnumerable<OrderDto> Orders);

