

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
        
        plant.SetName(request.Name, request.ModifiedBy);
        plant.SetDescription(request.Description, request.ModifiedBy);
        if (!request.IsActive)
        {
            plant.Deactivate(request.ModifiedBy);
        }

        foreach (var categoryId in plant.CategoryIds)
        {
            if (!request.CategoryIds.Contains(categoryId))
            {
                plant.RemoveCategory(categoryId, request.ModifiedBy);
            }
        }
        if (request.CategoryIds.Any())
        {
            foreach (var categoryId in request.CategoryIds)
            {
                if (!plant.CategoryIds.Contains(categoryId))
                {
                    plant.AddCategory(categoryId, request.ModifiedBy);
                }
            }
        }
        var result =  await context.UpdatePlantAsync(plant, cancellationToken);

        return new UpdatePlantResult(result);
    }
}
