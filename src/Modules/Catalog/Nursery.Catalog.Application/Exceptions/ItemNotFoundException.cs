
using BuildingBlocks.Exceptions;

namespace Nursery.Catalog.Application.Exceptions;

public class ItemNotFoundException : NotFoundException
{
    public ItemNotFoundException(string message) :base(message)
    {
        
    }
}
