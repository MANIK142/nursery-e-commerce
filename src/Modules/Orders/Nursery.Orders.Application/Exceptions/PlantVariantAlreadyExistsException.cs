
using BuildingBlocks.Exceptions;

namespace Nursery.Catalog.Application.Exceptions;

public class PlantVariantAlreadyExistsException : BadRequestException
{
    public PlantVariantAlreadyExistsException(string message):base(message) 
    {
        
    }
}
