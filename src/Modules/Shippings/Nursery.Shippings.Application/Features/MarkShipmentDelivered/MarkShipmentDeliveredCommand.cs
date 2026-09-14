using BuildingBlocks.Common.CQRS;
using FluentValidation;


namespace Nursery.Shippings.Application.Features.MarkShipmentDelivered;

public sealed record MarkShipmentDeliveredCommand(Guid ShipmentId) : ICommand<bool>;


public sealed class MarkShipmentDeliveredCommandValidator : AbstractValidator<MarkShipmentDeliveredCommand>
{
    public MarkShipmentDeliveredCommandValidator() =>
        RuleFor(c => c.ShipmentId).NotEmpty();
}
