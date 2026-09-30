
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Nursery.Web.Host.Models.DTOs.Identity;
using Nursery.Web.Host.Models.ViewModels;
using Nursery.Web.Host.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Nursery.Web.Host.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly IIdentityApiClient _identityApi;
        public AccountController(IIdentityApiClient identityApi)
        {
            this._identityApi = identityApi;
        }
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM, string? returnUrl = null,CancellationToken ct = default)
        {
            var request = new LoginRequest()
            {
                userName = loginVM.Email,
                password = loginVM.Password,
            };
            var result = await _identityApi.LoginAync(request, ct);

          

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "Login failed.");
                return View(loginVM);
            }

            var tokens = result.Response;
            var jwt = result.Response!.jwtToken; // adjust to your LoginResponse property name
            var refreshToken = result.Response!.RefreshToken;
            var ExpriesAtUtc = result.Response!.ExpiresAtUtc;


            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var identity = new ClaimsIdentity(token.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = loginVM.RememberMe,
                ExpiresUtc = token.ValidTo
            };


            authProperties.StoreTokens(new[]
        {
                new AuthenticationToken { Name = AuthTokenNames.AccessToken,  Value = tokens.jwtToken },
                new AuthenticationToken { Name = AuthTokenNames.RefreshToken, Value = tokens.RefreshToken },
                new AuthenticationToken { Name = AuthTokenNames.ExpiresAt,    Value = tokens.ExpiresAtUtc.ToString("o") }
            });


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

            //principal.Claims.FirstOrDefault()

            if (Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);


            if (principal.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = await HttpContext.GetTokenAsync(AuthTokenNames.RefreshToken);
            if (!string.IsNullOrEmpty(refreshToken))
            {
                try { await _identityApi.LogoutAsync(refreshToken, HttpContext.RequestAborted); }   // revokes the token family server-side
                catch (HttpRequestException) { /* best effort: still sign out locally if the API is down */ }
            }


            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}

public static class AuthTokenNames
{
    public const string AccessToken = "access_token";
    public const string RefreshToken = "refresh_token";
    public const string ExpiresAt = "expires_at";
}