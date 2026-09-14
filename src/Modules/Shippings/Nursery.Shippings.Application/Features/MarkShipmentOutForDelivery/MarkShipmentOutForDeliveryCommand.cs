using BuildingBlocks.Common.CQRS;
using FluentValidation;
using Nursery.Shippings.Application.Features.MarkShipmentPacked;

namespace Nursery.Shippings.Application.Features.MarkShipmentOutForDelivery;

public sealed record MarkShipmentOutForDeliveryCommand(Guid ShipmentId) : ICommand<bool>;


public sealed class MarkShipmentOutForDeliveryValidator : AbstractValidator<MarkShipmentOutForDeliveryCommand>
{
    public MarkShipmentOutForDeliveryValidator() =>
        RuleFor(c => c.ShipmentId).NotEmpty();
}
