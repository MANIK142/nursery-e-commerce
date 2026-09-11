using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Dto;

public class OrderItemDto
{
    public Guid Id { get;  set; }
    public Guid OrderId { get;  set; }
    public Guid PlantVariantId { get;  set; }
    public string ProductNameAtPurchase { get; set; } = default!;
    public int Quantity { get;  set; }
    public decimal UnitPriceAtPurchase { get; set; }
}
