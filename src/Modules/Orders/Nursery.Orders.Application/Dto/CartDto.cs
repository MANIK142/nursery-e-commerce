using Nursery.Orders.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Dto;

public record CartDto(Guid Id, Guid CustomerId, CartStatus CartStatus, decimal TotalPrice, List<CartItemDto> CartItemDtos);

