using BuildingBlocks.Common.CQRS;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Features.MarkShipmentShipped;

public sealed record MarkShipmentShippedCommand(
    Guid ShipmentId,
    string Carrier,
    string TrackingNumber,
    string? TrackingUrl) : ICommand<bool>;

public sealed class MarkShipmentShippedCommandValidator : AbstractValidator<MarkShipmentShippedCommand>
{
    public MarkShipmentShippedCommandValidator()
    {
        RuleFor(c => c.ShipmentId).NotEmpty();
        RuleFor(c => c.Carrier).NotEmpty().MaximumLength(100);
        RuleFor(c => c.TrackingNumber).NotEmpty().MaximumLength(100);
        RuleFor(c => c.TrackingUrl).MaximumLength(500);
    }
}