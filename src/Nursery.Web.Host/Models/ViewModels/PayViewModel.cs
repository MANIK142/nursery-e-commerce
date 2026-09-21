using Nursery.Web.Host.Models.DTOs.Orders;

namespace Nursery.Web.Host.Models.ViewModels;

public class PayViewModel
{
    public OrderDto Order { get; set; } = new();
    public string? ClientSecret { get; set; }
    public string PublishableKey { get; set; } = "";
    public string ReturnUrl { get; set; } = "";
    public string? Error { get; set; }
}

public sealed class ConfirmationViewModel
{
    public OrderDto Order { get; set; } = new();
}