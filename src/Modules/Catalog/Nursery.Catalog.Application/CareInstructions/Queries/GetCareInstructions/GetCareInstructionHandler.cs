using Microsoft.EntityFrameworkCore;


namespace Nursery.Catalog.Application.CareInstructions.Queries.GetCareInstructions;

public class GetCareInstructionHandler(ICatalogDbContext context) : IQueryHandler<GetCareInstructionQuery, GetCareInstructionResponse>
{
    private readonly ICatalogDbContext context = context;
    public async Task<GetCareInstructionResponse> Handle(GetCareInstructionQuery request, CancellationToken cancellationToken)
    {
        if(request.Id != null)
        {
            var careInstructions = await context.CareInstructions
                              .Where(p => p.PlantId == request.Id)
                              .Select(p => new CareInstructionDto
                              {
                                  PlantId = p.PlantId,
                                  AdditionalNotes = p.AdditionalNotes,
                                  DifficultyLevel  = p.DifficultyLevel,
                                  FertilizingFrequency = p.FertilizingFrequency,
                                  HumidityLevel = p.HumidityLevel,
                                  IsToxicToPets = p.IsToxicToPets,
                                  MaxTemperatureCelsius = p.MaxTemperatureCelsius,
                                  MinTemperatureCelsius = p.MaxTemperatureCelsius,
                                  PruningNotes = p.PruningNotes,
                                  SoilType = p.SoilType,
                                  SunlightRequirement = p.SunlightRequirement,
                                  WateringFrequency = p.WateringFrequency,
                              })
                              .ToListAsync();
            return new GetCareInstructionResponse(careInstructions);
        }

        var query = context.CareInstructions.AsQueryable();

        //if (!string.IsNullOrWhiteSpace(request.FilterValue) && !string.IsNullOrWhiteSpace(request.FilterBy))
        //{
        //    if (request.FilterBy?.ToLower() == "difficultylevel")
        //    {
        //        query = query.Where(c => c.DifficultyLevel != null && c.DifficultyLevel.Contains(request.FilterValue));
        //    }
        //    else if (request.FilterBy?.ToLower() == "description")
        //    {
        //        query = query.Where(c => c.Description != null && c.Description.Contains(request.FilterValue));
        //    }
        //    else if (request.FilterBy?.ToLower() == "category")
        //    {
        //        var categories = await context.Categories.Where(c => c.Name != null && c.Name.Contains(request.FilterValue)).ToListAsync();
                
        //        var PlantIds = await context.PlantCategories
        //            .Where(pc => categories.Select(c => c.Id).Contains(pc.CategoryId))
        //            .Select(pc => pc.PlantId)
        //            .ToListAsync();
     
        //        query = query.Where(c => PlantIds.Contains(c.Id));
        //    }
        //}

        var pageNumber = request.PageNumber ?? 0;
        var pageSize = request.PageSize ?? 10;

        var FilteredCareInstructions = await query
                  .OrderBy(c => c.CreatedAt)
                  .Skip(pageNumber * pageSize)
                  .Take(pageSize)
                  .Select(p => new CareInstructionDto
                  {
                      PlantId = p.PlantId,
                      AdditionalNotes = p.AdditionalNotes,
                      DifficultyLevel = p.DifficultyLevel,
                      FertilizingFrequency = p.FertilizingFrequency,
                      HumidityLevel = p.HumidityLevel,
                      IsToxicToPets = p.IsToxicToPets,
                      MaxTemperatureCelsius = p.MaxTemperatureCelsius,
                      MinTemperatureCelsius = p.MaxTemperatureCelsius,
                      PruningNotes = p.PruningNotes,
                      SoilType = p.SoilType,
                      SunlightRequirement = p.SunlightRequirement,
                      WateringFrequency = p.WateringFrequency,
                  })
                  .ToListAsync();
        return new GetCareInstructionResponse(FilteredCareInstructions);
    }
}
