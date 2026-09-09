
using BuildingBlocks.Exceptions;

namespace Nursery.Catalog.Application.Exceptions;

public class CustomerNotFoundException : NotFoundException
{
    public CustomerNotFoundException(string message) :base(message)
    {
        
    }
}
