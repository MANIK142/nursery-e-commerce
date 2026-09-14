
using Nursery.Shippings.Domain.Models;

namespace Nursery.Shippings.Application.Data;

public  interface IShipmentRepository
{
    Task AddAsync(Shipment shipment,CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<Shipment?> GetByIdAsync(Guid ShipmentId, CancellationToken cancellationToken);
}
