
using BuildingBlocks.Common.CQRS;
using Nursery.Orders.Application.Dto;
using System.Windows.Input;

namespace Nursery.Orders.Application.Features.GetAllOrder;

public record GetAllOrderQuery(int? PageNumber, int? PageSize, Guid? Id, string? FilterBy, string? FilterValue) : ICommand<GetAllOrderResult>;

public record GetAllOrderResult(IEnumerable<OrderDto> Orders);

