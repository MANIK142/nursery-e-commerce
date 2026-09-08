namespace Nursery.Catalog.Application.Plants.Commands.CreatePlant;

public class CreatePlantHandler(ICatalogRepository catalogRepository) : ICommandHandler<CreatePlantCommand, CreatePlantResponse>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;
    public async Task<CreatePlantResponse> Handle(CreatePlantCommand request, CancellationToken cancellationToken)
    {

        var plant = Plant.Create(
                request.Name,
                request.Description,
                request.CreatedBy,
                request.plantVariantSpecs,
                request.Images
            );

        foreach (var catId in request.Categories) {
            plant.AddCategory(catId, "admin@nursery.com");
        }
        
       var resposne = await catalogRepository.CreatePlantAsync(plant, cancellationToken);

       return new CreatePlantResponse(resposne.Id);
    }
}
