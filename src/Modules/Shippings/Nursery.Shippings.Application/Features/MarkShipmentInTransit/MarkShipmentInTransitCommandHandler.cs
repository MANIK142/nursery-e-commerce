
using BuildingBlocks.Common.CQRS;
using BuildingBlocks.Exceptions;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Domain.Models;

namespace Nursery.Shippings.Application.Features.MarkShipmentInTransit;

public class MarkShipmentInTransitCommandHandler(IShipmentRepository repository) : ICommandHandler<MarkShipmentInTransitCommand, bool>
{
    public async Task<bool> Handle(MarkShipmentInTransitCommand request, CancellationToken ct)
    {
        var shipment = await repository.GetByIdAsync(request.ShipmentId, ct)
          ?? throw new NotFoundException(nameof(Shipment), request.ShipmentId);

        shipment.MarkInTransit();

        await repository.SaveChangesAsync(ct);
        return true;
    }
}
