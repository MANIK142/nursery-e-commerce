using BuildingBlocks.Exceptions;


namespace Nursery.Orders.Application.Exceptions;

public class CancellationException : BadRequestException
{
    public CancellationException(string message) :base(message)
    {
        
    }
}
