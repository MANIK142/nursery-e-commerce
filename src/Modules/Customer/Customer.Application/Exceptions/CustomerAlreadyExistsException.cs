
using BuildingBlocks.Exceptions;

namespace Nursery.Catalog.Application.Exceptions;

public class CustomerAlreadyExistsException : BadRequestException
{
    public CustomerAlreadyExistsException(string message):base(message) 
    {
        
    }
}
