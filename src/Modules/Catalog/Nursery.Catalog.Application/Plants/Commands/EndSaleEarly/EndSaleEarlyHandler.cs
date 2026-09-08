using Nursery.Catalog.Application.Exceptions;
using Nursery.Catalog.Application.Plants.Commands.AddVariant;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Catalog.Application.Plants.Commands.EndSaleEarly;

public class EndSaleEarlyHandler(ICatalogRepository catalogRepository) : ICommandHandler<EndSaleEarlyCommand, EndSaleEarlyResult>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;

    public async Task<EndSaleEarlyResult> Handle(EndSaleEarlyCommand request, CancellationToken cancellationToken)
    {
        var plant = await catalogRepository.GetPlantById(request.PlantId, cancellationToken);
        if (plant == null)
        {
            throw new ItemNotFoundException($"Plant not found for id {request.PlantId}");
        }

        var variant = plant.Variants.FirstOrDefault(v => v.Id == request.VariantId);
        if (variant == null)
        {
            throw new ItemNotFoundException($"Variant not found for id {request.VariantId}");
        }

        variant.EndSaleEarly(request.Modifiedby);

        var result = await catalogRepository.UpdatePlantAsync(plant, cancellationToken);

        return new EndSaleEarlyResult(result);
    }
}
