using BuildingBlocks.Common.CQRS;
using MediatR;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Application.Plants.Commands.CreatePlant;

public class CreatePlantHandler(ICatalogRepository catalogRepository) : ICommandHandler<CreatePlantCommand, CreatePlantResponse>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;
    public async Task<CreatePlantResponse> Handle(CreatePlantCommand request, CancellationToken cancellationToken)
    {
        var plant = Plant.Create(
            request.SkuCode,
            request.Name,
            request.Description,
            request.RetailPrice,
            request.ImageUrl,
            request.CreatedBy);

        foreach (var catId in request.Categories) {
            plant.AddCategory(catId, "admin@nursery.com");
        }
        
       var resposne = await catalogRepository.CreatePlantAsync(plant, cancellationToken);

       return new CreatePlantResponse(resposne.Id);
    }
}
