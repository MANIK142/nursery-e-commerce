

using BuildingBlocks.Common.IntegrationEvents;
using MediatR;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Application.Features.CreateShipment;

namespace Nursery.Shippings.Application.EventHandler;

public sealed class PaymentConfirmedHandler(
    ISender sender,
    IOrderLineItemLookup orderLineItemLookup)
    : INotificationHandler<PaymentSucceededNotification>
{
    public async Task Handle(PaymentSucceededNotification notification, CancellationToken ct)
    {
        var orderLines = await orderLineItemLookup.GetLineItemsAsync(notification.OrderId, ct);
        await sender.Send(new CreateShipmentCommand(
            notification.OrderId,
            orderLines.Select(l => new ShipmentLineItemRequest(l.OrderItemId, l.ProductId, l.Quantity)).ToList()),
            ct);
    }
}
