
namespace Nursery.Catalog.Application.Plants.Queries.GetPlants;
public record GetPlantQuery(int? PageNumber,int? PageSize, Guid? Id,string? FilterBy,string? FilterValue) : IQuery<GetPlantResponse>;
public record GetPlantResponse(
    IEnumerable<PlantDto> Plants
);


