
using Microsoft.AspNetCore.Http.HttpResults;
using Nursery.Catalog.Application.Exceptions;

namespace Nursery.Catalog.Application.Plants.Commands.AddVariant;

public class AddVariantHandler(ICatalogRepository catalogRepository) : ICommandHandler<AddVariantCommand, AddVariantResult>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;

    public async Task<AddVariantResult> Handle(AddVariantCommand request, CancellationToken cancellationToken)
    {
        var plant = await catalogRepository.GetPlantById(request.PlantId, cancellationToken);
        if(plant == null)
        {
            throw new ItemNotFoundException($"Plant not found for id {request.PlantId}");
        }

        var  plantVariantSpecPlantId = new PlantVariantSpecWithPlantId(plant.Id, request.Sku, request.VariantName,
                                                    request.RetailPrice, request.WholesalePrice);

        var variant = plant.AddVariant(plantVariantSpecPlantId,request.ModifiedBy);

        var result = await catalogRepository.UpdatePlantAsync(plant, cancellationToken);

        return new AddVariantResult(variant.Id);
    }
}
