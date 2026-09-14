using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Payment.Api.Contracts;
public interface IOrderLookup
{
    Task<OrderPaymentInfo?> GetOrderPaymentInfoAsync(Guid orderId, CancellationToken ct);
}

public sealed record OrderPaymentInfo(Guid OrderId, Guid CustomerId, decimal TotalAmount, string Status);
