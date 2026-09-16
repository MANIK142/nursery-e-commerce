
using BuildingBlocks.Common.CQRS;
using Microsoft.EntityFrameworkCore;
using Nursery.Catalog.Application.Data;
using Nursery.Catalog.Domain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Nursery.Catalog.Application.Catagories.Queries.GetCategory;

public record GetCategoryQuery(int? PageNumber, int? PageSize,Guid? Id, string? FilterBy, string? FilterByValue) : IQuery<GetCategoryResult>;
public record GetCategoryResult(IEnumerable<CategoryDto> Categories);
public class GetQueryHandler(ICatalogDbContext catalogDbContext) : IQueryHandler<GetCategoryQuery, GetCategoryResult>
{
    private readonly ICatalogDbContext catalogDbContext = catalogDbContext;

    public async Task<GetCategoryResult> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        
        if(request.Id != null)
        {
            var category = await catalogDbContext.Categories.Select(c => new CategoryDto(c.Id,c.Name)).FirstOrDefaultAsync(c => c.Id == request.Id);
            return new GetCategoryResult(new[] { category });
        }
        var query = catalogDbContext.Categories.AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.FilterByValue) && !string.IsNullOrWhiteSpace(request.FilterBy))
        {
            if (request.FilterBy?.ToLower() == "name")
            {
              query = query.Where(c => c.Name != null && c.Name.Contains(request.FilterByValue));
            } else if (request.FilterBy?.ToLower() == "description")
            {
              query = query.Where(c => c.Description != null && c.Description.Contains(request.FilterByValue));
            }
        }


        var pageNumber = request.PageNumber ?? 0;
        var pageSize = request.PageSize ?? 10;

        var categories = await query
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CategoryDto(c.Id, c.Name))
                    .Skip(pageNumber * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        return new GetCategoryResult(categories);
    }
}
