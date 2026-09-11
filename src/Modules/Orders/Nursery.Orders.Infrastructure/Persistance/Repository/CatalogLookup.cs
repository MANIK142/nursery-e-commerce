

using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Domain.Models;
using Nursery.Catalog.Infrastructure.Persistence.Context;
using Nursery.Orders.Application.Data;

namespace Nursery.Orders.Infrastructure.Persistance.Repository;

public class CatalogLookup(CatalogDbContext catalogDb) : ICatalogLookup
{
    public async Task<PlantVariant?> GetPlantVariantById(Guid PlantVariantId, CancellationToken cancellationToken)
    {
        return await catalogDb.PlantVariants.Include(pv => pv.SalePrices)
                              .FirstOrDefaultAsync(pv => pv.Id == PlantVariantId);
    }
}
