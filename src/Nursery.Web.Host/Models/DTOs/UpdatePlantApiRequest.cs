namespace Nursery.Web.Host.Models.DTOs;
public record UpdatePlantApiRequest(
    Guid Id,
    string Name,
    string Description,
    string ModifiedBy,
    List<string> Categories,
    List<PlantVariantSpecDto> PlantVariantSpecs,
    List<ImageSpecDto> Images
);

