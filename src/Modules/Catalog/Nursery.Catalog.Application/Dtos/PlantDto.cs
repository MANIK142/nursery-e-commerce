
using System.Reflection.Emit;

namespace Nursery.Catalog.Application.Dtos;
public class PlantDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<PlantImageDto> PlantImages { get; set; } = default!;
    public List<CategoryDto> Categories { get; set; } = new();
    public List<PlantVariantDto> PlantVariants { get; set; } = new();
    public CareInstructionDto? CareInstruction { get; set; } 
}
