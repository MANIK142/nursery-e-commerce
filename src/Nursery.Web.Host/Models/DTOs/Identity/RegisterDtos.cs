namespace Nursery.Web.Host.Models.DTOs.Identity;

public class RegisterRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;

    public List<string> Roles { get; set; } = default!;

}

public class RegisterResponse
{
    public string status { get; set; } = default!;
    public string message { get; set; } = default!;

}

