

using BuildingBlocks.Common.CQRS;
using FluentValidation;
using Nursery.Catalog.Application.Plants.Commands.CreatePlant;

namespace Nursery.Catalog.Application.Catagories.Command.CreateCatagory;

public record CreateCategoryCommand(string Name, string Description, string CreatedBy) : ICommand<CreateCategoryResut>;
public record CreateCategoryResut(Guid Id);


public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        RuleFor(x => x.Description).MaximumLength(250).WithMessage("Description must not exceed 250 characters");;
    }
}
