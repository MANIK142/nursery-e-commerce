using BuildingBlocks.Common.CQRS;
using BuildingBlocks.Exceptions;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Features.CancelShipment
{
    internal class CancelShipmentCommandHandler(IShipmentRepository repository) : ICommandHandler<CancelShipmentCommand, bool>
    {
        public async Task<bool> Handle(CancelShipmentCommand request, CancellationToken ct)
        {
            var shipment = await repository.GetByIdAsync(request.ShipmentId, ct)
              ?? throw new NotFoundException(nameof(Shipment), request.ShipmentId);

            shipment.Cancel();

            await repository.SaveChangesAsync(ct);
            return true;
        }
    }
}
