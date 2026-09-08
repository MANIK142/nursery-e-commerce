

using FluentValidation;

namespace Nursery.Catalog.Application.Plants.Commands.RemoveVariant;

public record RemoveVariantCommand(Guid VariantId, Guid PlantId,string ModifiedBy) : ICommand<RemoveVariantResult>;
public record RemoveVariantResult(bool IsSuccess);

public class RemoveVariantValidator : AbstractValidator<RemoveVariantCommand>
{
    public RemoveVariantValidator()
    {
        RuleFor(x => x.VariantId).NotEmpty().WithMessage("Variant Id cannot be null");
        RuleFor(x => x.PlantId).NotEmpty().WithMessage("Plant Id cannot be null");
    }
}