namespace Nursery.Web.Host.Models.Identity;
public class LoginRequest
{
    public string userName { get; set; } = default!;
    public string password { get; set; } = default!;
}

public class LoginResponse
{
    public string jwtToken { get; set; }
}

public record LoginResult(bool IsSuccess, LoginResponse? Response, string? Error)
{
    public static LoginResult Succeeded(LoginResponse response) => new(true, response, null);
    public static LoginResult Failed(string error) => new(false, null, error);
}