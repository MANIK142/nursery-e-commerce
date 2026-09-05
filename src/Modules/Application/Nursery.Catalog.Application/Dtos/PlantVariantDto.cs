
namespace Nursery.Catalog.Application.Dtos;

public class PlantVariantDto
{
    public string Sku { get; private set; } = default!;
    public string VariantName { get; private set; } = default!;
    public Money RetailPrice { get; private set; } = default!;
    public Money WholesalePrice { get; private set; } = default!;
}
