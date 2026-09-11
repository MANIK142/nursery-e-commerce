using BuildingBlocks.Common.CQRS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Nursery.Orders.Application.Features.CancelOrder;

public record CancelOrderCommand(Guid OrderId,Guid CustomerId) : ICommand<CancelOrderResult>;

public record CancelOrderResult(bool IsSuccess);

