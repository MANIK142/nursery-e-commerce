
using BuildingBlocks.Common.CQRS;
using FluentValidation;
using MediatR;
using Nursery.Shippings.Application.Features.MarkShipmentPacked;

namespace Nursery.Shippings.Application.Features.CancelShipment;

public sealed record CancelShipmentCommand(Guid ShipmentId) : ICommand<bool>;

public sealed class CancelShipmentCommandValidator : AbstractValidator<CancelShipmentCommand>
{
    public CancelShipmentCommandValidator() =>
        RuleFor(c => c.ShipmentId).NotEmpty();
}
