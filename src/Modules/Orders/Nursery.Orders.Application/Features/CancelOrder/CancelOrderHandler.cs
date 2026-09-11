
using BuildingBlocks.Common.CQRS;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Application.Exceptions;
using Nursery.Orders.Domain.Enums;

namespace Nursery.Orders.Application.Features.CancelOrder;

public class CancelOrderHandler(IOrderRepository repository) : ICommandHandler<CancelOrderCommand, CancelOrderResult>
{
    public async Task<CancelOrderResult> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.OrderId,cancellationToken)
                            ?? throw new OrderNotFoundException($"Order not found for this Order id {request.OrderId}");
        if(order.Status == OrderStatus.Confirmed)
        {
            order.CancelOrder();
            var result = await repository.SaveChangesAsync(cancellationToken);
            return new CancelOrderResult(result);
        }
        throw new CancellationException($"Order Cannot be cancelled once item is Shipped!! Return request can be made once items are delivered to get refund");
    }
}
