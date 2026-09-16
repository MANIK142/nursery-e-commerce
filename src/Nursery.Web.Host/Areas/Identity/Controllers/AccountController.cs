
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Nursery.Web.Host.Models.Identity;
using Nursery.Web.Host.Services.Interface;
using Nursery.Web.Host.ViewModels;
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
            var jwt = result.Response!.jwtToken; // adjust to your LoginResponse property name
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwt);

            var identity = new ClaimsIdentity(token.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = loginVM.RememberMe,
                ExpiresUtc = token.ValidTo
            };
            authProperties.StoreTokens(new[] { new AuthenticationToken { Name = "access_token", Value = jwt } });

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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }

}
