using BuildingBlocks.Common.CQRS;
using FluentValidation;



namespace Nursery.Shippings.Application.Features.MarkShipmentFailedDelivery;

public sealed record MarkShipmentFailedDeliveryCommand(Guid ShipmentId) : ICommand<bool>;

public sealed class MarkShipmentFailedDeliveryCommandValidator : AbstractValidator<MarkShipmentFailedDeliveryCommand>
{
    public MarkShipmentFailedDeliveryCommandValidator() =>
        RuleFor(c => c.ShipmentId).NotEmpty();
}
