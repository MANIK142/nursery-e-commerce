
using BuildingBlocks.Exceptions;

namespace Nursery.Orders.Application.Exceptions;

public class OrderNotFoundException : NotFoundException
{
    public OrderNotFoundException(string message) : base(message)
    {
        
    }

}
