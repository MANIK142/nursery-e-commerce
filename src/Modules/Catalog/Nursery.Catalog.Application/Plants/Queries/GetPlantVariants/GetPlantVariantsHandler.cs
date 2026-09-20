
using Microsoft.EntityFrameworkCore;

namespace Nursery.Catalog.Application.Plants.Queries.GetPlantVariants;

public class GetPlantVariantsHandler(ICatalogDbContext dbContext) : IQueryHandler<GetPlantVariantsQuery, GetPlantVaraintsResult>
{
    public async Task<GetPlantVaraintsResult> Handle(GetPlantVariantsQuery request, CancellationToken cancellationToken)
    {
        var ids = request.PlantVariantIds?.Distinct().ToList();

        if (ids == null || ids.Count == 0)
        {
            return new GetPlantVaraintsResult(new List<PlantVariantDto>());
        }

        var plantVariants = await dbContext.PlantVariants
            .AsNoTracking()
            .Include(pv => pv.SalePrices)
            .Include(pv => pv.Images)
            .Where(v => ids.Contains(v.Id))
            .Select(v => new PlantVariantDto()
            {
                Id = v.Id,
                PlantId = v.PlantId,
                Sku = v.Sku,
                VariantName = v.VariantName,
                RetailPrice = v.RetailPrice,
                SalePrices =v.SalePrices.Select(sp => new SalePriceDto()
                {
                    Id=sp.Id,
                    SalePrice =sp.SalePrice,
                    PlantVariantId =sp.PlantVariantId,
                    StartsAtUtc = sp.StartsAtUtc,
                    EndsAtUtc = sp.EndsAtUtc,
                    IsActiveAt =sp.IsActiveAt(DateTime.UtcNow)
                }).ToList(),
                WholesalePrice = v.WholesalePrice,
                Price = v.GetPrice(CustomerTier.Retail,DateTime.UtcNow),
                Images = v.Images.Select(i => new PlantImageDto()
                {
                    Id= i.Id,
                    AltText = i.AltText,
                    IsPrimaryImage = i.IsPrimaryImage,
                    StorageKey = i.StorageKey,
                }).ToList()
             }).ToListAsync();


        return new GetPlantVaraintsResult(plantVariants);
    }
}
