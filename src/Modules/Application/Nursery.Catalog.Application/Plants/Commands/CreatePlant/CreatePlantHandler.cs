namespace Nursery.Catalog.Application.Plants.Commands.CreatePlant;

public class CreatePlantHandler(ICatalogRepository catalogRepository) : ICommandHandler<CreatePlantCommand, CreatePlantResponse>
{
    private readonly ICatalogRepository catalogRepository = catalogRepository;
    public async Task<CreatePlantResponse> Handle(CreatePlantCommand request, CancellationToken cancellationToken)
    {
        //List<ImageSpec> imageSpecs = [];
        //foreach(var image in request.Images)
        //{
            // Upload Image to Server/Cloud
            //var storageKey = Guid.NewGuid().ToString();
            //var imageSpec = new ImageSpec(storageKey,image.IsPrimary,image.AltName);
        //    imageSpecs.Add(image);
        //}

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
