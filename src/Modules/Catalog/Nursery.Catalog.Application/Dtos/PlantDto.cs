
using System.Reflection.Emit;

namespace Nursery.Catalog.Application.Dtos;
public class PlantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<PlantImage> PlantImages { get; set; } = default!;
    public List<CategoryDto> Categories { get; set; } = new();
    public List<PlantVariant> PlantVariants { get; set; } = new();
    public CareInstruction? CareInstruction { get; set; } 
}
