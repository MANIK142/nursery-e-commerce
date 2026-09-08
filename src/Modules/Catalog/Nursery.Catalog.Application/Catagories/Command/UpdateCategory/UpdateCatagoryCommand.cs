

using BuildingBlocks.Common.CQRS;
using FluentValidation;
using Nursery.Catalog.Application.Plants.Commands.CreatePlant;

namespace Nursery.Catalog.Application.Catagories.Command.UpdateCategory;

public record UpdateCategoryCommand(Guid Id, string Name, string Description, string UpdatedBy) : ICommand<UpdateCategoryResult>;
public record UpdateCategoryResult(bool IsSuccess);


public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        RuleFor(x => x.Description).MaximumLength(250).WithMessage("Description must not exceed 250 characters");;
    }
}
