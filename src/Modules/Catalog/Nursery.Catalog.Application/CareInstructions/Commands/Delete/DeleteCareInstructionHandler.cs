
using Nursery.Catalog.Application.Exceptions;

namespace Nursery.Catalog.Application.CareInstructions.Commands.Delete;

public class DeleteCareInstructionHandler(ICatalogRepository catalog) : ICommandHandler<DeleteCareInstructionCommand, DeleteCareInstructionResult>
{
    private readonly ICatalogRepository catalog = catalog;

    public async Task<DeleteCareInstructionResult> Handle(DeleteCareInstructionCommand request, CancellationToken cancellationToken)
    {
        var careInstruction = await catalog.GetCareInstructionByPlantId(request.PlantId,cancellationToken);
        if (careInstruction == null)
            throw new ItemNotFoundException($"Care Instructions not found for Plant Id {request.PlantId}");

        var result = await catalog.DeleteCareInstructionByPlantId(request.PlantId, cancellationToken);

        return new DeleteCareInstructionResult(result);
    }
}
