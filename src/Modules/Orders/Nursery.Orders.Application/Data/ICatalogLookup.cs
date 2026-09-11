using Nursery.Catalog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Orders.Application.Data;

public interface ICatalogLookup
{
    Task<PlantVariant> GetPlantVariantById(Guid PlantVariantId,CancellationToken cancellationToken);
}
