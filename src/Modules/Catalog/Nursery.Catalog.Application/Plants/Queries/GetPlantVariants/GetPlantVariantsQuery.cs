
namespace Nursery.Catalog.Application.Plants.Queries.GetPlantVariants;

public record GetPlantVariantsQuery(List<Guid> PlantVariantIds) : IQuery<GetPlantVaraintsResult>;

public record GetPlantVaraintsResult(List<PlantVariantDto> PlantVariants);
