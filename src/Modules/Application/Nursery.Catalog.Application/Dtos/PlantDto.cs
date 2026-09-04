
namespace Nursery.Catalog.Application.Dtos;
public class PlantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string SkuCode { get; set; } = default!;
    public decimal RetailPrice { get; set; }
    public List<CategoryDto> Categories { get; set; } = new();
}
