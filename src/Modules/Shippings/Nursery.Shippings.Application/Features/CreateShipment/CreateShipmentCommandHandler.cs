

using MediatR;
using Microsoft.EntityFrameworkCore;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Domain.Models;

namespace Nursery.Shippings.Application.Features.CreateShipment;

public sealed class CreateShipmentCommandHandler(
    IShipmentRepository shipmentRepository,
    IShippingDbContext dbContext,
    IOrderLineItemLookup orderLineItemLookup)
    : IRequestHandler<CreateShipmentCommand, Guid>
{
    public async Task<Guid> Handle(CreateShipmentCommand request, CancellationToken ct)
    {
        var orderLines = await orderLineItemLookup.GetLineItemsAsync(request.OrderId, ct);
        var orderLinesById = orderLines.ToDictionary(l => l.OrderItemId);

        // Sum quantities already shipped across ALL existing shipments for this order —
        // this is the check the aggregate itself can't do (see domain-layer note).
        //var alreadyShippedByOrderItem = await dbContext.Shipments
        //                                        .AsNoTracking()
        //                                        .Where(s => s.OrderId == request.OrderId)
        //                                        .SelectMany(s => s.LineItems)
        //                                        .GroupBy(li => li.OrderItemId)
        //                                        .Select(g => new { OrderItemId = g.Key, Quantity = g.Sum(x => x.Quantity) })
        //                                        .ToDictionaryAsync(x => x.OrderItemId, x => x.Quantity, ct);

        //foreach (var line in request.LineItems)
        //{
        //    if (!orderLinesById.TryGetValue(line.OrderItemId, out var orderLine))
        //        throw new InvalidOperationException(
        //            $"OrderItem {line.OrderItemId} does not belong to Order {request.OrderId}.");

        //    var alreadyShipped = alreadyShippedByOrderItem.GetValueOrDefault(line.OrderItemId, 0);
        //    var remaining = orderLine.Quantity - alreadyShipped;

        //    if (line.Quantity > remaining)
        //        throw new InvalidOperationException(
        //            $"Cannot ship {line.Quantity} of OrderItem {line.OrderItemId}; only {remaining} remaining.");
        //}
        try
        {
            var shipment = Shipment.Create(
                      Guid.NewGuid(),
                      request.OrderId,
                      request.LineItems.Select(l => (l.OrderItemId, l.ProductId, l.Quantity)));

            await shipmentRepository.AddAsync(shipment, ct);
            await shipmentRepository.SaveChangesAsync(ct);

            return shipment.Id;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return Guid.Empty;
        }
      
    }
}