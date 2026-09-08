using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nursery.Identity.DatabaseContext;
using Nursery.Identity.Models.Domain;
using Nursery.Identity.Repository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<NurseryIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDb"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;              // Must have at least one number (0-9)
    options.Password.RequireLowercase = true;          // Must have at least one lowercase letter (a-z)
    options.Password.RequireUppercase = true;          // Must have at least one uppercase letter (A-Z)
    options.Password.RequireNonAlphanumeric = true;    // Must have at least one special character (!@#$%^&*)
    options.Password.RequiredLength = 7;               // Minimum 7 characters
    options.Password.RequiredUniqueChars = 1;          // Minimum unique characters
})
.AddEntityFrameworkStores<NurseryIdentityDbContext>();

builder.Services.AddScoped<ITokenRepository, TokenRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
