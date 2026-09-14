using BuildingBlocks.Common.CQRS;
using BuildingBlocks.Exceptions;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Domain.Models;


namespace Nursery.Shippings.Application.Features.MarkShipmentOutForDelivery;

public class MarkShipmentOutForDeliveryCommandHandler(IShipmentRepository repository) : ICommandHandler<MarkShipmentOutForDeliveryCommand, bool>
{
    public async Task<bool> Handle(MarkShipmentOutForDeliveryCommand request, CancellationToken ct)
    {
        var shipment = await repository.GetByIdAsync(request.ShipmentId, ct)
           ?? throw new NotFoundException(nameof(Shipment), request.ShipmentId);

        shipment.MarkOutForDelivery();

        await repository.SaveChangesAsync(ct);

        return true;
    }
}
