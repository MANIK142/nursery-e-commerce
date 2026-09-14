using BuildingBlocks.Common.CQRS;
using FluentValidation;



namespace Nursery.Shippings.Application.Features.MarkShipmentPacked;

public sealed record MarkShipmentPackedCommand(Guid ShipmentId) : ICommand<bool>;


public sealed class MarkShipmentPackedCommandValidator : AbstractValidator<MarkShipmentPackedCommand>
{
    public MarkShipmentPackedCommandValidator() =>
        RuleFor(c => c.ShipmentId).NotEmpty();
}