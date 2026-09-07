

using BuildingBlocks.Common.CQRS;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Application.Plants.Commands.CreatePlant;

public record CreatePlantCommand(string SkuCode, string Name, string Description, decimal RetailPrice, 
    string CreatedBy, List<Guid> Categories, List<PlantVariantSpec> plantVariantSpecs, List<ImageSpec> Images) : ICommand<CreatePlantResponse>;

public record CreatePlantResponse(Guid Id);

public class CreatePlantCommandValidator : AbstractValidator<CreatePlantCommand>
{
    public CreatePlantCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Name).MaximumLength(100).WithMessage("Name must not exceed 100 characters");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
        RuleFor(x => x.Description).MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");
        RuleFor(x =>x.Categories).NotNull().WithMessage("Category cannot be empty");
        RuleFor(x => x.plantVariantSpecs).NotNull().WithMessage("Atleast one Variants needed for adding a plant");
        //RuleFor(x => x.ImageSpecs).NotNull().WithMessage("Atleast one Plant Image needed for adding a plant");
    }
}