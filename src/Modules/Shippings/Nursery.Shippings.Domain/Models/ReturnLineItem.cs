
using BuildingBlocks.Common;
using Nursery.ShareKernel;

namespace Nursery.Shippings.Domain.Models;

public class ReturnLineItem : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid ShipmentLineItemId { get; private set; }
    public int Quantity { get; private set; }

    private ReturnLineItem() { } // EF Core

    internal ReturnLineItem(Guid id, Guid shipmentLineItemId, int quantity)
    {
        Guard.AgainstDefault(shipmentLineItemId, nameof(shipmentLineItemId));
        Guard.AgainstNegativeOrZero(quantity, nameof(quantity));

        Id = id;
        ShipmentLineItemId = shipmentLineItemId;
        Quantity = quantity;
    }
}
