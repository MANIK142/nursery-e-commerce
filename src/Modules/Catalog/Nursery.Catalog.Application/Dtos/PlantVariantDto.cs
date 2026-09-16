
namespace Nursery.Catalog.Application.Dtos;

public class PlantVariantDto
{
    public Guid Id { get;  set; }
    public Guid PlantId { get;  set; }
    public string Sku { get;  set; } = default!;
    public string VariantName { get;  set; } = default!;
    public Money RetailPrice { get;  set; } = default!;
    public Money Price { get; set; } = default!;
    public List<PlantImageDto> Images { get; set; } = default!;
}
