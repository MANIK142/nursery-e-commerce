

using BuildingBlocks.Common.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Nursery.Orders.Application.Data;
using Nursery.Orders.Domain.Enums;

namespace Nursery.Orders.Application.EventHandlers;

public class PaymentSucceededHandler(IOrderRepository orderRepository, ILogger<PaymentSucceededHandler> logger) : INotificationHandler<PaymentSucceededNotification>
{
    public async Task Handle(PaymentSucceededNotification notification, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(notification.OrderId, cancellationToken);
        if (order is null)
        {
            logger.LogWarning(
                "PaymentSucceeded received for unknown Order {OrderId} (Payment {PaymentId}).",
                notification.OrderId, notification.PaymentId);
            return;
        }

        order.MarkAsPaid();
        await orderRepository.SaveChangesAsync(cancellationToken);

    }
}
