
using BuildingBlocks.Common.Caching;

namespace Nursery.Catalog.Application.Plants.Queries.GetPlants;
public record GetCareInstructionQuery(int? PageNumber,int? PageSize, Guid? Id,string? FilterBy,string? FilterValue) : IQuery<GetPlantResponse>, ICacheableQuery
{
    public string CacheKey => $"catalog:plants:list:page={PageNumber}:size={PageSize}:id={Id}:filterBy={FilterBy}:filterValue={FilterValue}";
    public TimeSpan? Expiration => TimeSpan.FromMinutes(10);
}

public record GetPlantResponse(
    IEnumerable<PlantDto> Plants
);


