using BuildingBlocks.Common.CQRS;
using FluentValidation;
using MediatR;
using Nursery.Shippings.Application.Features.MarkShipmentPacked;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Application.Features.MarkShipmentInTransit;

public sealed record MarkShipmentInTransitCommand(Guid ShipmentId) : ICommand<bool>;

public sealed class MarkShipmentInTransitCommandValidator : AbstractValidator<MarkShipmentInTransitCommand>
{
    public MarkShipmentInTransitCommandValidator() =>
        RuleFor(c => c.ShipmentId).NotEmpty();
}