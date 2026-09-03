
using BuildingBlocks.Exceptions;

namespace Nursery.Catalog.Application.Exceptions;

public class ItemAlreadyExistsException : BadRequestException
{
    public ItemAlreadyExistsException(string message):base(message) 
    {
        
    }
}
