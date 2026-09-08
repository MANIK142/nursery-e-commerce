

using Nursery.Catalog.Application.Exceptions;

namespace Nursery.Catalog.Application.Plants.Commands.AddSale;

public class AddSaleHandler(ICatalogRepository catalog) : ICommandHandler<AddSaleCommand, AddSaleResult>
{
    private readonly ICatalogRepository catalogRepository = catalog;

    public async Task<AddSaleResult> Handle(AddSaleCommand request, CancellationToken cancellationToken)
    {
        var plant = await catalogRepository.GetPlantById(request.PlantId, cancellationToken);
        if (plant == null)
        {
            throw new ItemNotFoundException($"Plant not found for plant Id {request.PlantId}");
        }
        var variant = plant.Variants.FirstOrDefault(v => v.Id == request.PlantVariantId);
        if (variant == null)
        {
            throw new ItemNotFoundException($"Plant Variant not found for variant Id {request.PlantVariantId}");
        }

        variant.StartSale(request.SalePrice, request.StartsAtUtc, request.EndsAtUtc, request.ModifiedBy);

        var result = await catalogRepository.UpdatePlantAsync(plant,cancellationToken);

        return new AddSaleResult(result);
    }
}
