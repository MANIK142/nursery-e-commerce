using BuildingBlocks.Common;
using Nursery.ShareKernel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Domain.Models;

public sealed class ShipmentLineItem : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid OrderItemId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    private ShipmentLineItem() { } // EF Core

    internal ShipmentLineItem(Guid id, Guid orderItemId, Guid productId, int quantity)
    {
        Guard.AgainstDefault(orderItemId, nameof(orderItemId));
        Guard.AgainstDefault(productId, nameof(productId));
        Guard.AgainstNegativeOrZero(quantity, nameof(quantity));

        Id = id;
        OrderItemId = orderItemId;
        ProductId = productId;
        Quantity = quantity;
    }
}