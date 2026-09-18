

using Nursery.Catalog.Application.Exceptions;

namespace Nursery.Catalog.Application.Plants.Commands.UpdatePlant;
public class UpdatePlantHandler(ICatalogRepository context) : ICommandHandler<UpdatePlantCommand, UpdatePlantResult>
{
    private readonly ICatalogRepository context = context;
    public async Task<UpdatePlantResult> Handle(UpdatePlantCommand request, CancellationToken cancellationToken)
    {
        var plant = await context.GetPlantById(request.Id, cancellationToken);
        if (plant == null)
        {
            throw new ItemNotFoundException($"Plant with ID {request.Id} not found");
        }
        
        plant.SetName(request.Name, request.Name);
        plant.SetDescription(request.Description, request.Description);

        if (request.Categories.Any())
        {
            foreach (var categoryId in request.Categories)
            {
                if (!plant.Categories.Select(c => c.CategoryId).Contains(categoryId))
                {
                    plant.AddCategory(categoryId, request.ModifiedBy);
                }
            }
        }

        var categoriesToRemove = plant.Categories
            .Where(c => request.Categories == null || !request.Categories.Contains(c.CategoryId))
            .Select(c => c.CategoryId)
            .ToList();

        foreach (var categoryId in categoriesToRemove)
        {
            plant.RemoveCategory(categoryId, request.ModifiedBy);
        }

        if (request.Images.Any())
        {
            foreach(var plantimage in request.Images)
            {
                if (!plant.Images.Select(p => p.StorageKey).ToList().Contains(plantimage.StorageKey))
                {
                    plant.AddPlantImage(plant.Id, plantimage, request.ModifiedBy);
                }
            }
        }

        var plantImagesToRemove = plant.Images
                                    .Where(c => request.Images == null || !request.Images.Select(i => i.StorageKey)
                                    .Contains(c.StorageKey))
                                    .Select(c => c.Id)
                                    .ToList();
        foreach(var imaageId in plantImagesToRemove)
        {
            plant.RemovePlantImage(imaageId, request.ModifiedBy);
        }


        if (request.PlantVariantSpecs.Any())
        {
            foreach (var spec in request.PlantVariantSpecs)
            {
                //if (!plant.Variants.Select(p => p.Sku).ToList().Contains(plantVariantSpec.Sku))
                //{
                //    var variant = new PlantVariantSpecWithPlantId(plant.Id, plantVariantSpec.Sku, plantVariantSpec.VariantName, 
                //                                                    plantVariantSpec.ImageSpecs, plantVariantSpec.RetailPrice, plantVariantSpec.WholesalePrice);

                //    plant.AddVariant(variant, request.ModifiedBy);
                //}
                var existingVariant = plant.Variants.FirstOrDefault(v => v.Sku == spec.Sku);

                if (existingVariant != null)
                {
                    // 1. UPDATE EXISTING VARIANT
                    var updateSpec = new PlantVariantSpecWithId(
                        existingVariant.Id,
                        spec.Sku,
                        spec.VariantName,
                        spec.RetailPrice,
                        spec.WholesalePrice
                    );

                    plant.UpdateVariant(updateSpec, request.ModifiedBy);
                }
                else
                {
                    // 2. ADD NEW VARIANT
                    var newSpec = new PlantVariantSpecWithPlantId(
                        plant.Id,
                        spec.Sku,
                        spec.VariantName,
                        spec.ImageSpecs,
                        spec.RetailPrice,
                        spec.WholesalePrice
                    );

                    plant.AddVariant(newSpec, request.ModifiedBy);
                }
            }
        }


        var plantVariantToRemove = plant.Variants
                               .Where(c => request.PlantVariantSpecs == null || !request.PlantVariantSpecs.Select(i => i.Sku)
                               .Contains(c.Sku))
                               .Select(c => c.Id)
                               .ToList();
        foreach (var variantId in plantVariantToRemove)
        {
            plant.RemoveVariant(variantId,request.ModifiedBy);
        }
        


        var result =  await context.UpdatePlantAsync(plant, cancellationToken);

        return new UpdatePlantResult(result);
    }
}
