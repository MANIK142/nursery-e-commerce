

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


        var ExistingCategories = await context.GetCategoriesByPlantId(plant.Id, cancellationToken);
        if (ExistingCategories != null)
        {
            foreach (var categoryId in ExistingCategories)
            {
                if (!request.Categories.Contains(categoryId))
                {
                    plant.RemoveCategory(categoryId, request.ModifiedBy);
                }
            }
        }

        if (request.Categories.Any())
        {
            foreach (var categoryId in request.Categories)
            {
                if (!ExistingCategories.Contains(categoryId))
                {
                    plant.AddCategory(categoryId, request.ModifiedBy);
                }
            }
        }

    
        var result =  await context.UpdatePlantAsync(plant, cancellationToken);

        return new UpdatePlantResult(result);
    }
}
