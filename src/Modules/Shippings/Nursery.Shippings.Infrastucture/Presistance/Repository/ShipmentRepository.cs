using Microsoft.EntityFrameworkCore;
using Nursery.Shippings.Application.Data;
using Nursery.Shippings.Domain.Models;
using Nursery.Shippings.Infrastucture.Presistance.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Infrastucture.Presistance.Repository;

public class ShipmentRepository(ShippingDbContext dbContext) : IShipmentRepository
{
    public async Task AddAsync(Shipment shipment, CancellationToken cancellationToken)
    {
        await dbContext.Shipments.AddAsync(shipment, cancellationToken);
    }

    public async Task<Shipment?> GetByIdAsync(Guid ShipmentId, CancellationToken cancellationToken)
    {
        return await dbContext.Shipments.FirstOrDefaultAsync(s => s.Id == ShipmentId, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
       await SaveChangesAsync(cancellationToken);
    }
}
