using Microsoft.EntityFrameworkCore;


namespace Nursery.Catalog.Application.Plants.Queries.GetPlants;

public class GetCareInstructionHandler(ICatalogDbContext context) : IQueryHandler<GetCareInstructionQuery, GetPlantResponse>
{
    private readonly ICatalogDbContext context = context;
    public async Task<GetPlantResponse> Handle(GetCareInstructionQuery request, CancellationToken cancellationToken)
    {
        if(request.Id != null)
        {
            var plant = await context.Plants
                              .Where(p => p.IsActive && p.Id == request.Id)
                              .Include(p => p.Images)
                              .Include(p => p.CareInstruction)
                              .Select(p => new PlantDto
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  Description = p.Description,
                                  PlantImages = p.Images.ToList(),
                                  Categories = context.PlantCategories
                                      .Where(pc => pc.PlantId == p.Id)
                                      .Join(context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => new CategoryDto(c.Id, c.Name)).ToList(),
                                  PlantVariants = context.PlantVariants
                                                    .Where(pv => pv.PlantId == p.Id).Include(pv => pv.Images).Include(p =>p.SalePrices).ToList(),
                                  CareInstruction = p.CareInstruction               
                              })
                              .ToListAsync();
            return new GetPlantResponse(plant);
        }

        var query = context.Plants
                    .Where(p => p.IsActive).Include(p => p.Images).Include(p => p.CareInstruction).AsQueryable();

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
                      Description = p.Description,
                      PlantImages = p.Images.ToList(),
                      Categories = context.PlantCategories
                            .Where(pc => pc.PlantId == p.Id)
                            .Join(context.Categories, pc => pc.CategoryId, c => c.Id, (pc, c) => new CategoryDto(c.Id, c.Name)).ToList(),
                      PlantVariants = context.PlantVariants
                                                    .Where(pv => pv.PlantId == p.Id).Include(pv => pv.Images).Include(p => p.SalePrices).ToList(),
                      CareInstruction = p.CareInstruction
                  })
                  .ToListAsync();
        return new GetPlantResponse(FilteredPlants);
    }
}
