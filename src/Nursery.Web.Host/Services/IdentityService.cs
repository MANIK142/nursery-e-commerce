
using Microsoft.AspNetCore.Mvc;
using Nursery.Web.Host.Models;
using Nursery.Web.Host.Models.DTOs.Catalog;
using Nursery.Web.Host.Models.DTOs.Identity;
using Nursery.Web.Host.Services.Interface;
using System.Security.Principal;

namespace Nursery.Web.Host.Services;

public class IdentityService(HttpClient httpClient) : IIdentityApiClient
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<ApiResultModel<LoginResponse>?> LoginAync(LoginRequest loginRequest, CancellationToken ct)
    {
   
        var response = await _httpClient.PostAsJsonAsync("api/Auth/Login", loginRequest, ct);

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
            return ApiResultModel<LoginResponse>.Failed(problem?.Detail ?? $"Login failed ({(int)response.StatusCode})");
        }
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>(cancellationToken: ct);
        return ApiResultModel<LoginResponse>.Succeeded(result!);
       
    }

    public async Task<ApiResultModel<RegisterResponse>?> RegisterAsync(RegisterRequest registerRequest, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync("api/Auth/Register", registerRequest, ct);
        response.EnsureSuccessStatusCode();

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
            return ApiResultModel<RegisterResponse>.Failed(problem?.Detail ?? $"Login failed ({(int)response.StatusCode})");
        }
        var result = await response.Content.ReadFromJsonAsync<RegisterResponse>(cancellationToken: ct);
        return ApiResultModel<RegisterResponse>.Succeeded(result!);
    }
}
