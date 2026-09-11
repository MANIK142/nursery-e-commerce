

using BuildingBlocks.Common.CQRS;
using FluentValidation;
using Nursery.Orders.Application.Dto;
using Nursery.Orders.Domain.ValueObjects;


namespace Nursery.Orders.Application.Features.CreateOrder;

public record CreateOrderCommand(Guid CustomerId, AddressDto BillingAddress, AddressDto ShippingAddress,List<CreateOrderItemDto> Items) : ICommand<CreateOrderResponse>;

public record CreateOrderResponse(Guid OrderId);

public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderValidator()
    {
        RuleFor(x => x.CustomerId)
           .NotEmpty();

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Order must contain at least one item.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.PlantVariantId)
                .NotEmpty();

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0);
        });

        RuleFor(x => x.ShippingAddress)
            .NotNull()
            .SetValidator(new AddressDtoValidator());

        RuleFor(x => x.BillingAddress)
            .NotNull()
            .SetValidator(new AddressDtoValidator());
    
    }
}

public class AddressDtoValidator : AbstractValidator<AddressDto>
{
    public AddressDtoValidator()
    {
        RuleFor(a => a.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(a => a.LastName).NotEmpty().MaximumLength(100);
        RuleFor(a => a.EmailAddress).EmailAddress().When(a => !string.IsNullOrWhiteSpace(a.EmailAddress));
        RuleFor(a => a.AddressLine).NotEmpty().MaximumLength(200);
        RuleFor(a => a.Country).NotEmpty().MaximumLength(100);
        RuleFor(a => a.State).NotEmpty().MaximumLength(100);
        RuleFor(a => a.ZipCode).NotEmpty().MaximumLength(20);
    }
}