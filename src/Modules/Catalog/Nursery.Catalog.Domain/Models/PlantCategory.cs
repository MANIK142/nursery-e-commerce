
namespace Nursery.Catalog.Domain.Models;
public class PlantCategory
{
    public Guid PlantId { get; private set; }
    public Guid CategoryId { get; private set; }

    private PlantCategory() { }
    private PlantCategory(Guid plantId, Guid categoryId)
    {
        PlantId = plantId;
        CategoryId = categoryId;
    }

    public static PlantCategory Create(Guid plantId, Guid categoryId)
    {
        return new PlantCategory(plantId, categoryId);
    }
}
