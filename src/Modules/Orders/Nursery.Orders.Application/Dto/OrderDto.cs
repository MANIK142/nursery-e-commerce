using Nursery.Orders.Domain.Enums;
using Nursery.Orders.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Dto;

public class OrderDto
{
    public Guid Id { get; set; }
    public Guid CustomerId { get;  set; }
    public List<OrderItemDto> OrderItems { get; set; } = [];
    public OrderStatus Status { get;  set; }
    public PaymentStatus PaymentStatus { get;  set; }
}
