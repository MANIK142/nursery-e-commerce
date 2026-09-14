

using BuildingBlocks.Common.CQRS;
using FluentValidation;
using MediatR;

namespace Nursery.Shippings.Application.Features.CreateShipment;

public sealed record ShipmentLineItemRequest(Guid OrderItemId, Guid ProductId, int Quantity);

public sealed record CreateShipmentCommand(
    Guid OrderId,
    IReadOnlyList<ShipmentLineItemRequest> LineItems) : ICommand<Guid>;

public sealed class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
{
    public CreateShipmentCommandValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();

        RuleFor(c => c.LineItems)
            .NotEmpty()
            .WithMessage("A shipment must contain at least one line item.");

        RuleForEach(c => c.LineItems).ChildRules(line =>
        {
            line.RuleFor(l => l.OrderItemId).NotEmpty();
            line.RuleFor(l => l.ProductId).NotEmpty();
            line.RuleFor(l => l.Quantity).GreaterThan(0);
        });
    }
}