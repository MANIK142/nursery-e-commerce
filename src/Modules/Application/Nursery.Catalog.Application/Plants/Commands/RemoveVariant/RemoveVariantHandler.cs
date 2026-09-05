

using Nursery.Catalog.Application.Exceptions;

namespace Nursery.Catalog.Application.Plants.Commands.RemoveVariant;

public class RemoveVariantHandler(ICatalogRepository catalogRepository) : ICommandHandler<RemoveVariantCommand, RemoveVariantResult>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;

    public async Task<RemoveVariantResult> Handle(RemoveVariantCommand request, CancellationToken cancellationToken)
    {
        var plant = await catalogRepository.GetPlantById(request.PlantId, cancellationToken);
        if (plant == null)
        {
            throw new ItemNotFoundException($"Plant not found for plant Id {request.PlantId}");
        }
        var variant = plant.Variants.FirstOrDefault(v => v.Id == request.VariantId);
        if (variant == null)
        {
            throw new ItemNotFoundException($"Plant Variant not found for variant Id {request.VariantId}");
        }

        plant.RemoveVariant(request.VariantId, request.ModifiedBy);
        var result = await catalogRepository.UpdatePlantAsync(plant,cancellationToken);
        return new RemoveVariantResult(result);
    }
}
