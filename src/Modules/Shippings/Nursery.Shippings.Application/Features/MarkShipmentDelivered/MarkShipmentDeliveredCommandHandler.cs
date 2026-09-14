

using BuildingBlocks.Common.CQRS;
using BuildingBlocks.Exceptions;
using MediatR;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Domain.Models;

namespace Nursery.Shippings.Application.Features.MarkShipmentDelivered;

public class MarkShipmentDeliveredCommandHandler(IShipmentRepository repository, IPublisher publisher) : ICommandHandler<MarkShipmentDeliveredCommand, bool>
{
    public async Task<bool> Handle(MarkShipmentDeliveredCommand request, CancellationToken ct)
    {
        var shipment = await repository.GetByIdAsync(request.ShipmentId, ct)
            ?? throw new NotFoundException(nameof(Shipment), request.ShipmentId);

        shipment.MarkDelivered();

        await repository.SaveChangesAsync(ct);

        // Orders module needs this to update order status / notify customer.
        //await publisher.Publish(new ShipmentDelivered(shipment.OrderId, shipment.Id), ct);
        return true;
    }
}
