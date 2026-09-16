
namespace Nursery.Catalog.Application.Dtos;

public class PlantImageDto
{
    public  Guid Id { get; set; } 
    public string AltText { get;  set; } = default!;
    public string StorageKey { get;  set; } = default!;
    public bool IsPrimaryImage { get;  set; }
}
