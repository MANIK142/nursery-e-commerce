
using BuildingBlocks.Exceptions;

namespace Nursery.Catalog.Application.Exceptions;

public class PlantVariantNotFoundException : NotFoundException
{
    public PlantVariantNotFoundException(string message) :base(message)
    {
        
    }
}
