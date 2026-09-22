using Microsoft.AspNetCore.Authentication.Cookies;
using Nursery.Web.Host.Services;
using Nursery.Web.Host.Services.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddTransient<JwtForwardingHandler>();

builder.Services.AddHttpClient<ICatalogApiClient, CatalogApiClient>((sp, client) =>
{
    var baseUrl = sp.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"]
        ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");
    client.BaseAddress = new Uri(baseUrl);
}).AddHttpMessageHandler<JwtForwardingHandler>();

builder.Services.AddHttpClient<IIdentityApiClient, IdentityService>((sp, client) =>
{
    var baseUrl = sp.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"]
         ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");
    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddHttpClient<IOrderApiClient, OrderApiClient>((sp, client) =>
{
    var baseUrl = sp.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"]
         ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");
    client.BaseAddress = new Uri(baseUrl);
}).AddHttpMessageHandler<JwtForwardingHandler>();


builder.Services.AddHttpClient<ICustomerApi, CustomerApi>((sp, client) =>
{
    var baseUrl = sp.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"]
         ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");
    client.BaseAddress = new Uri(baseUrl);
}).AddHttpMessageHandler<JwtForwardingHandler>();


builder.Services.AddHttpClient<ICheckoutApi, CheckoutApi>((sp, client) =>
{
    var baseUrl = sp.GetRequiredService<IConfiguration>()["ApiSettings:BaseUrl"]
         ?? throw new InvalidOperationException("ApiSettings:BaseUrl is not configured.");
    client.BaseAddress = new Uri(baseUrl);
}).AddHttpMessageHandler<JwtForwardingHandler>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;

        options.Cookie.HttpOnly = true; // default is already true — explicit for clarity
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
        options.Cookie.SameSite = SameSiteMode.Lax; // or Strict, depending on cross-site needs
        options.Cookie.Name = "Nursery.Auth";

    });


var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}",
    defaults: new { area = "Customer" })
    .WithStaticAssets();


app.Run();


//stripe listen --forward - to https://localhost:7139/api/v1/payments/webhook --all-snapshot