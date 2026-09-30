namespace Nursery.Identity.Models.DTO;
public sealed record RefreshTokenResult(string AccessToken, DateTime AccessTokenExpiresAtUtc, string RefreshToken);