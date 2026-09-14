using BuildingBlocks.Common;
using Nursery.ShareKernel;
using Nursery.Shippings.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Domain.Models;

public sealed class Return :BaseDomainModel
{
    public Guid Id { get; private set; }
    public Guid ShipmentId { get; private set; }
    public ReturnStatus Status { get; private set; }
    public ReturnReason Reason { get; private set; }
    public DateTimeOffset RequestedAt { get; private set; }

    private readonly List<ReturnLineItem> _lineItems = [];
    public IReadOnlyCollection<ReturnLineItem> LineItems => _lineItems.AsReadOnly();
    private Return() { } // EF Core

    private Return(Guid id, Guid shipmentId, ReturnReason reason)
    {
        Id = id;
        ShipmentId = shipmentId;
        Reason = reason;
        Status = ReturnStatus.Requested;
        RequestedAt = DateTimeOffset.UtcNow;
    }

    // Caller (Application handler) must have already verified:
    //  1. shipment.Status == Delivered
    //  2. no other active Return exists against the same ShipmentLineItems
    //  3. requested quantities <= shipped quantities - already-returned quantities
    public static Return Create(
        Guid id,
        Guid shipmentId,
        ReturnReason reason,
        IEnumerable<(Guid ShipmentLineItemId, int Quantity)> lines)
    {
        Guard.AgainstDefault(shipmentId, nameof(shipmentId));

        var ret = new Return(id, shipmentId, reason);

        foreach (var line in lines)
        {
            ret._lineItems.Add(new ReturnLineItem(Guid.NewGuid(), line.ShipmentLineItemId, line.Quantity));
        }

        Guard.AgainstNegativeOrZero(ret._lineItems.Count, nameof(lines));

        return ret;
    }

    internal void Approve()
    {
        if (Status != ReturnStatus.Requested)
            throw new InvalidOperationException($"Cannot approve a return in {Status} status.");

        Status = ReturnStatus.Approved;
    }

    internal void Reject()
    {
        if (Status != ReturnStatus.Requested)
            throw new InvalidOperationException($"Cannot reject a return in {Status} status.");

        Status = ReturnStatus.Rejected;
    }

    internal void SchedulePickup()
    {
        if (Status != ReturnStatus.Approved)
            throw new InvalidOperationException($"Cannot schedule pickup from {Status} status.");

        Status = ReturnStatus.PickupScheduled;
    }

    internal void MarkInTransit()
    {
        if (Status != ReturnStatus.PickupScheduled)
            throw new InvalidOperationException($"Cannot mark in-transit from {Status} status.");

        Status = ReturnStatus.InTransit;
    }

    internal void MarkReceived()
    {
        if (Status != ReturnStatus.InTransit)
            throw new InvalidOperationException($"Cannot mark received from {Status} status.");

        Status = ReturnStatus.Received;
    }

    internal void MarkRefunded()
    {
        if (Status != ReturnStatus.Received)
            throw new InvalidOperationException($"Cannot refund from {Status} status.");

        Status = ReturnStatus.Refunded;
    }

    internal void Close()
    {
        if (Status is not (ReturnStatus.Refunded or ReturnStatus.Rejected))
            throw new InvalidOperationException($"Cannot close a return in {Status} status.");

        Status = ReturnStatus.Closed;
    }
}
