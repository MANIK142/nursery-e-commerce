
namespace Nursery.Catalog.Application.Dtos;

public class SalePriceDto
{
    public Guid Id { get;  set; }
    public Guid PlantVariantId { get;  set; }
    public Money SalePrice { get;  set; } = default!;
    public DateTime StartsAtUtc { get;  set; }
    public DateTime EndsAtUtc { get;  set; }
    public bool IsActiveAt { get; set; }
}
