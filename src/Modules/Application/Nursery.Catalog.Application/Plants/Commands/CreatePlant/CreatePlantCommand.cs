

using BuildingBlocks.Common.CQRS;
using FluentValidation;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Application.Plants.Commands.CreatePlant;

public record CreatePlantCommand(string SkuCode, string Name, string Description, decimal RetailPrice, string ImageUrl, string CreatedBy) : ICommand<CreatePlantResponse>;

public record CreatePlantResponse(Plant Plant);

public class CreatePlantCommandValidator : AbstractValidator<CreatePlantCommand>
{
    public CreatePlantCommandValidator()
    {
        RuleFor(x => x.SkuCode).NotEmpty().WithMessage("Sku Code is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");
        RuleFor(x => x.RetailPrice).GreaterThan(0).WithMessage("Retail Price must be a positive number");
    }
}