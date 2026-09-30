using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;

namespace Nursery.Web.Host.Auth;

public static class TokenRefreshEvents
{
    private static readonly TimeSpan RefreshWindow = TimeSpan.FromSeconds(60);

    public static async Task ValidatePrincipalAsync(CookieValidatePrincipalContext ctx)
    {
        var refreshToken = ctx.Properties.GetTokenValue(AuthTokenNames.RefreshToken);
        if (!DateTimeOffset.TryParse(ctx.Properties.GetTokenValue(AuthTokenNames.ExpiresAt),
                CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var expiresAt)
            || string.IsNullOrEmpty(refreshToken))
        {
            await RejectAsync(ctx);
            return;
        }

        if (expiresAt - DateTimeOffset.UtcNow > RefreshWindow) return;   // still fresh

        var refresher = ctx.HttpContext.RequestServices.GetRequiredService<ITokenRefresher>();
        var result = await refresher.RefreshAsync(refreshToken, ctx.HttpContext.RequestAborted);
        if (result is null)
        {
            await RejectAsync(ctx);
            return;
        }

        ctx.Properties.UpdateTokenValue(AuthTokenNames.AccessToken, result.jwtToken);
        ctx.Properties.UpdateTokenValue(AuthTokenNames.RefreshToken, result.RefreshToken);
        ctx.Properties.UpdateTokenValue(AuthTokenNames.ExpiresAt, result.ExpiresAtUtc.ToString("o"));
        ctx.ShouldRenew = true;
    }

    private static async Task RejectAsync(CookieValidatePrincipalContext ctx)
    {
        ctx.RejectPrincipal();
        await ctx.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}