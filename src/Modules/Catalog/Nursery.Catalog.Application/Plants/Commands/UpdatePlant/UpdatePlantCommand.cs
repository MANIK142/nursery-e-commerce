

using FluentValidation;
using Nursery.Catalog.Application.Plants.Commands.CreatePlant;

namespace Nursery.Catalog.Application.Plants.Commands.UpdatePlant;


public record UpdatePlantCommand(Guid Id,string SkuCode, string Name, string Description,string ModifiedBy, List<Guid> Categories,
                                List<PlantVariantSpec> PlantVariantSpecs, List<ImageSpec> Images) 
                                : ICommand<UpdatePlantResult>;

public record UpdatePlantResult(bool IsSuccess);

public class UpdatePlantCommandValidator : AbstractValidator<UpdatePlantCommand>
{
    public UpdatePlantCommandValidator()
    {
        RuleFor(x => x.SkuCode).NotEmpty().WithMessage("Sku Code is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");
    }
}
