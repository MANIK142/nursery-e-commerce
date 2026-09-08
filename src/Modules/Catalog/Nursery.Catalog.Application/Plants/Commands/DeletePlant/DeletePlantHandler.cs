

using Nursery.Catalog.Application.Exceptions;

namespace Nursery.Catalog.Application.Plants.Commands.DeletePlant;

public class DeletePlantHandler(ICatalogRepository context) : ICommandHandler<DeletePlantCommand, DeletePlantResult>
{
    private readonly ICatalogRepository context = context;
    public async Task<DeletePlantResult> Handle(DeletePlantCommand request, CancellationToken cancellationToken)
    {
        var plant = await context.GetPlantById(request.Id, cancellationToken); 
        if (plant == null)
        {
            throw new ItemNotFoundException($"Plant with ID {request.Id} not found");
        }

       var result = await context.DeletePlantById(request.Id, cancellationToken);
       return new DeletePlantResult(result);
    }
}
