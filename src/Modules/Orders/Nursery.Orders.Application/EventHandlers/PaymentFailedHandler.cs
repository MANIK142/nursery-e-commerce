using BuildingBlocks.Common.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using Nursery.Orders.Application.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.EventHandlers;

public sealed class PaymentFailedHandler(
    IOrderRepository orderRepository,
    ILogger<PaymentFailedHandler> logger)
    : INotificationHandler<PaymentFailedNotification>
{
    public async Task Handle(PaymentFailedNotification notification, CancellationToken ct)
    {
        var order = await orderRepository.GetByIdAsync(notification.OrderId, ct);

        if (order is null)
        {
            logger.LogWarning(
                "PaymentFailed received for unknown Order {OrderId} (Payment {PaymentId}): {Reason}",
                notification.OrderId, notification.PaymentId, notification.Reason);
            return;
        }

        order.MarkPaymentFailed();
        await orderRepository.SaveChangesAsync(ct);
    }
}