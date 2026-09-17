using Nursery.Web.Host.Models.DTOs.Identity;

namespace Nursery.Web.Host.Models;

public record ApiResultModel<T>(bool IsSuccess, T? Response, string? Error)
{
    public static ApiResultModel<T> Succeeded(T response) => new(true, response, null);
    public static ApiResultModel<T> Failed(string error) => new(false, default, error);
}

