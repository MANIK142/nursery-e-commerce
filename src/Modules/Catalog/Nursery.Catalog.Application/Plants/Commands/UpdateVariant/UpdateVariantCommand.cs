
using FluentValidation;

namespace Nursery.Catalog.Application.Plants.Commands.UpdateVariant;

public record UpdateVariantCommand(Guid Id, Guid PlantId, string Sku, string VariantName, Money RetailPrice,Money WholesalePrice, string ModifiedBy) : ICommand<UpdateVariantResult>;
public record UpdateVariantResult(bool IsSuccess);


public class UpdateVariantValidator : AbstractValidator<UpdateVariantCommand>
{
    public UpdateVariantValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Plant Id cannot be empty");

        RuleFor(x => x.PlantId).NotEmpty().WithMessage("Plant Id cannot be empty");

        RuleFor(x => x.Sku).NotEmpty().WithMessage("Sku Cannot be empty");
        RuleFor(x => x.Sku).MaximumLength(20).WithMessage("Sku length cannot exceed 1205 characters");

        RuleFor(x => x.VariantName).NotEmpty().WithMessage("Variant Name Cannot be empty");
        RuleFor(x => x.VariantName).MaximumLength(100).WithMessage("Sku length cannot exceed 100 characters");

        RuleFor(x => x.RetailPrice.Amount).GreaterThan(0).WithMessage("Retail Amount greater than Zero");

        RuleFor(x => x.WholesalePrice.Amount).LessThan(x => x.RetailPrice.Amount).WithMessage("Wholesale Price should be less than Retail Price");
        RuleFor(x => x.WholesalePrice.Amount).GreaterThan(0).WithMessage("Wholesale Amount greater than Zero");
    }
}