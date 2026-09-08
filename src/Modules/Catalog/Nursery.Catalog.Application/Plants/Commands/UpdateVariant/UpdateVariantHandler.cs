
using Nursery.Catalog.Application.Exceptions;

namespace Nursery.Catalog.Application.Plants.Commands.UpdateVariant;

public class UpdateVariantHandler(ICatalogRepository catalogRepository) : ICommandHandler<UpdateVariantCommand, UpdateVariantResult>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;

    public async Task<UpdateVariantResult> Handle(UpdateVariantCommand request, CancellationToken cancellationToken)
    {
        var plant = await catalogRepository.GetPlantById(request.PlantId,cancellationToken);
        if (plant == null)
        {
            throw new ItemNotFoundException($"Plant not found for plant Id {request.PlantId}");
        }
        var variant = plant.Variants.FirstOrDefault(v => v.Id == request.Id);
        if (variant == null)
        {
            throw new ItemNotFoundException($"Plant Variant not found for variant Id {request.Id}");
        }

        var variantSpec = new PlantVariantSpecWithId(request.Id, request.Sku, request.VariantName, request.RetailPrice, request.WholesalePrice);

        plant.UpdateVariant(variantSpec, request.ModifiedBy);

        var result = await catalogRepository.UpdatePlantAsync(plant,cancellationToken);

        return new UpdateVariantResult(result);


    }
}
