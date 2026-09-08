
namespace Nursery.Catalog.Application.Plants.Commands.DeletePlant;
public record DeletePlantCommand(Guid Id) : ICommand<DeletePlantResult>;
public record DeletePlantResult(bool IsSuccess);