
namespace Nursery.Catalog.Infrastructure.Exceptions;
public class BlobStorageException : Exception
{
    public BlobStorageException(string message ):base(message)
    {
        
    }
    public BlobStorageException(string message, Exception? ex) : base(message, ex)
    {

    }
}
