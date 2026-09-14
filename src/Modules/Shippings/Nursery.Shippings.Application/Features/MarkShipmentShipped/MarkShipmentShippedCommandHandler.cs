using BuildingBlocks.Common.CQRS;
using BuildingBlocks.Exceptions;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Domain.Models;


namespace Nursery.Shippings.Application.Features.MarkShipmentShipped;

public class MarkShipmentShippedCommandHandler(IShipmentRepository repository) : ICommandHandler<MarkShipmentShippedCommand, bool>
{
    public async Task<bool> Handle(MarkShipmentShippedCommand request, CancellationToken ct)
    {
        var shipment = await repository.GetByIdAsync(request.ShipmentId, ct)
          ?? throw new NotFoundException(nameof(Shipment), request.ShipmentId);

        var trackingInfo = TrackingInfo.Of(request.Carrier, request.TrackingNumber, request.TrackingUrl);
        shipment.MarkShipped(trackingInfo);

        await repository.SaveChangesAsync(ct);
        return true;
    }
}
