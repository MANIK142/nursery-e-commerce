
using FluentValidation;
using Nursery.Catalog.Application.Plants.Commands.UpdatePlant;

namespace Nursery.Catalog.Application.Plants.Commands.AddSale;
public record AddSaleCommand(Guid PlantId,Guid PlantVariantId, Money SalePrice, DateTime StartsAtUtc, DateTime EndsAtUtc, string ModifiedBy) : ICommand<AddSaleResult>;
public record AddSaleResult(bool IsSuccess);

public class AddSaleValidator : AbstractValidator<AddSaleCommand>
{
    public AddSaleValidator()
    {
        RuleFor(x => x.PlantId).NotEmpty();
        RuleFor(x => x.PlantVariantId).NotEmpty();

        RuleFor(x => x.SalePrice)
            .NotNull().WithMessage("Sale price is required.");

        RuleFor(x => x.SalePrice.Amount)
            .GreaterThan(0).WithMessage("Sale price must be greater than zero.")
            .When(x => x.SalePrice is not null);

        RuleFor(x => x.SalePrice.Currency)
            .NotEmpty()
            .Length(3).WithMessage("Currency must be a 3-letter ISO code.")
            .When(x => x.SalePrice is not null);

        RuleFor(x => x.StartsAtUtc)
            .GreaterThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Sale start time cannot be in the past.");

        RuleFor(x => x.EndsAtUtc)
            .GreaterThan(x => x.StartsAtUtc)
            .WithMessage("Sale end time must be after its start time.");

        RuleFor(x => x.ModifiedBy).NotEmpty();
    }
}