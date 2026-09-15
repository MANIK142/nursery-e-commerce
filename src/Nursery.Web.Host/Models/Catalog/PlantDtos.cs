namespace Nursery.Web.Host.Models.Catalog;



public record PlantSummaryDto(
    Guid Id,
    string Name,
    string? ThumbnailUrl,
    decimal Price,
    string Category
);

public record PlantDetailDto(
    Guid Id,
    string Name,
    string Description,
    string CareInstructions,
    decimal Price,
    string Category,
    IReadOnlyList<string> ImageUrls
);

public record PagedResult<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalCount
);
