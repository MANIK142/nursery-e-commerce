
using Microsoft.EntityFrameworkCore;
using Nursery.Shippings.Domain.Models;

namespace Nursery.Shippings.Application.Data;

public interface IShippingDbContext
{
    DbSet<Shipment> Shipments { get; }
    DbSet<ShipmentLineItem> ShipmentLineItems { get; }
    DbSet<Return> Returns { get; }
    DbSet<ReturnLineItem> ReturnLineItems { get; }
}
