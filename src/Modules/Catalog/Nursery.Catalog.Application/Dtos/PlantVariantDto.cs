
namespace Nursery.Catalog.Application.Dtos;

public class PlantVariantDto
{
    public Guid Id { get;  set; }
    public Guid PlantId { get;  set; }
    public string Sku { get;  set; } = default!;
    public string VariantName { get;  set; } = default!;
    public Money RetailPrice { get;  set; } = new();
    public Money WholesalePrice { get; set; } = new();
    public Money Price { get; set; } = new();
    public List<PlantImageDto> Images { get; set; } = new();
    public List<SalePriceDto> SalePrices { get; set; } = new();
}
