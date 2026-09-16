namespace Nursery.Web.Host.Services.Interface;

public interface ICurrentUserService
{
    bool IsAuthenticated { get; }
    string? CustomerId { get; }
    string? Email { get; }
    string? Name { get; }
}
