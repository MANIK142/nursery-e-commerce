
using BuildingBlocks.Common;
using Nursery.ShareKernel;
using Nursery.Shippings.Domain.Enums;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.Marshalling;

namespace Nursery.Shippings.Domain.Models;
public class Shipment : BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public TrackingInfo? TrackingInfo { get; private set; }
    public DateTimeOffset? ShippedAt { get; private set; }
    public DateTimeOffset? DeliveredAt { get; private set; }

    private readonly List<ShipmentLineItem> _lineItems = [];
    public IReadOnlyCollection<ShipmentLineItem> LineItems => _lineItems.AsReadOnly();

    private Shipment() { } // EF Core

    private Shipment(Guid id, Guid orderId)
    {
        Id = id;
        OrderId = orderId;
        Status = ShipmentStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    // Application layer has already validated ordered-vs-shipped quantities
    // via IOrderLineItemLookup before calling this.
    public static Shipment Create(
        Guid id,
        Guid orderId,
        IEnumerable<(Guid OrderItemId, Guid ProductId, int Quantity)> lines)
    {
        Guard.AgainstDefault(orderId, nameof(orderId));

        var shipment = new Shipment(id, orderId);

        foreach (var line in lines)
        {
            shipment._lineItems.Add(new ShipmentLineItem(
                Guid.NewGuid(), line.OrderItemId, line.ProductId, line.Quantity));
        }

        Guard.AgainstNegativeOrZero(shipment._lineItems.Count, nameof(lines)); // must have ≥1 line

        return shipment;
    }

    public void MarkPacked()
    {
        if (Status != ShipmentStatus.Pending)
            throw new InvalidOperationException($"Cannot pack a shipment in {Status} status.");

        Status = ShipmentStatus.Packed;
    }

    public void MarkShipped(TrackingInfo trackingInfo)
    {
        if (Status != ShipmentStatus.Packed)
            throw new InvalidOperationException($"Cannot ship a shipment in {Status} status.");

        TrackingInfo = trackingInfo;
        Status = ShipmentStatus.Shipped;
        ShippedAt = DateTimeOffset.UtcNow;
    }

    public void MarkInTransit()
    {
        if (Status != ShipmentStatus.Shipped)
            throw new InvalidOperationException($"Cannot mark in-transit from {Status} status.");

        Status = ShipmentStatus.InTransit;
    }

    public void MarkOutForDelivery()
    {
        if (Status != ShipmentStatus.InTransit)
            throw new InvalidOperationException($"Cannot mark out-for-delivery from {Status} status.");

        Status = ShipmentStatus.OutForDelivery;
    }

    public void MarkDelivered()
    {
        if (Status != ShipmentStatus.OutForDelivery)
            throw new InvalidOperationException($"Cannot deliver from {Status} status.");

        Status = ShipmentStatus.Delivered;
        DeliveredAt = DateTimeOffset.UtcNow;
    }

    public void MarkFailedDelivery()
    {
        if (Status is not (ShipmentStatus.OutForDelivery or ShipmentStatus.InTransit))
            throw new InvalidOperationException($"Cannot fail delivery from {Status} status.");

        Status = ShipmentStatus.FailedDelivery;
    }

    public void Cancel()
    {
        if (Status is not (ShipmentStatus.Pending or ShipmentStatus.Packed))
            throw new InvalidOperationException("Cannot cancel a shipment that has already been shipped.");

        Status = ShipmentStatus.Cancelled;
    }

    // Remaining quantity for a given order line, used by the Application-layer
    // lookup to compute already-shipped totals across all shipments for an order.
    public int GetQuantityFor(Guid orderItemId) =>
        _lineItems.Where(li => li.OrderItemId == orderItemId).Sum(li => li.Quantity);

}
