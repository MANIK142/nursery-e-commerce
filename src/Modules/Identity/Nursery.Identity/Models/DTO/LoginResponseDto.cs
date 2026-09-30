namespace Nursery.Identity.Models.DTO;

public class LoginResponseDto
{
    public string JwtToken { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime ExpiresAtUtc { get; set; } = default!;
}
