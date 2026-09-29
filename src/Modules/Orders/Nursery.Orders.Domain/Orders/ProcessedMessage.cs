using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Domain.Orders;

public class ProcessedMessage
{
    public Guid MessageId { get; set; }
    public DateTime ProcessedAtUtc { get; set; }
}
