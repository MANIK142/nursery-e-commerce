using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Nursery.Catalog.Application.Plants.Queries.GetPlants;

public class GetPlantHandler(ICatalogDbContext context) : IQueryHandler<GetPlantQuery, GetPlantResponse>
{
    private readonly ICatalogDbContext context = context;
    public async Task<GetPlantResponse> Handle(GetPlantQuery request, CancellationToken cancellationToken)
    {
        if(request.Id != null)
        {
            var plant = await context.Plants
                              .Where(p => p.IsActive && p.Id == request.Id)
                              .Select(p => new PlantDto
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  SkuCode = p.SkuCode,
                                  Description = p.Description,
                                  RetailPrice = p.RetailPrice,
                                  Categories = context.PlantCategories
                                      .Where(pc => pc.PlantId == p.Id)
                                      .Join(context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => new CategoryDto(c.Id, c.Name)).ToList()
                              })
                              .ToListAsync();
            return new GetPlantResponse(plant);
        }

        var query = context.Plants
                    .Where(p => p.IsActive).AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.FilterValue) && !string.IsNullOrWhiteSpace(request.FilterBy))
        {
            if (request.FilterBy?.ToLower() == "name")
            {
                query = query.Where(c => c.Name != null && c.Name.Contains(request.FilterValue));
            }
            else if (request.FilterBy?.ToLower() == "description")
            {
                query = query.Where(c => c.Description != null && c.Description.Contains(request.FilterValue));
            }
            else if (request.FilterBy?.ToLower() == "category")
            {
                var categories = await context.Categories.Where(c => c.Name != null && c.Name.Contains(request.FilterValue)).ToListAsync();
                
                var PlantIds = await context.PlantCategories
                    .Where(pc => categories.Select(c => c.Id).Contains(pc.CategoryId))
                    .Select(pc => pc.PlantId)
                    .ToListAsync();
     
                query = query.Where(c => PlantIds.Contains(c.Id));
            }
        }

        var pageNumber = request.PageNumber ?? 0;
        var pageSize = request.PageSize ?? 10;

        var FilteredPlants = await query
                  .OrderBy(c => c.CreatedAt)
                  .Skip(pageNumber * pageSize)
                  .Take(pageSize)
                  .Select(p => new PlantDto
                  {
                      Id = p.Id,
                      Name = p.Name,
                      SkuCode = p.SkuCode,
                      RetailPrice = p.RetailPrice,
                      Description = p.Description,
                      Categories = context.PlantCategories
                            .Where(pc => pc.PlantId == p.Id)
                            .Join(context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => new CategoryDto(c.Id, c.Name)).ToList()
                  })
                  .ToListAsync();
        return new GetPlantResponse(FilteredPlants);
    }
}
