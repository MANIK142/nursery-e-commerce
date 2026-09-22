using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nursery.Web.Host.Models.DTOs.Orders;
using Nursery.Web.Host.Models.ViewModels.Orders;
using Nursery.Web.Host.Services.Interface;
using System.Numerics;

namespace Nursery.Web.Host.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class OrderController(IOrderApiClient orderApi) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> getallorder(CancellationToken ct)
    {
        var orders = await orderApi.GetAllOrderAsync(ct);
        var flattenedOrders = orders.Select(p => p.ToFlatOrderDto()).ToList();
        return Json(new { data = flattenedOrders });
    }

    //[HttpGet("/admin/order/update/{Id:Guid}")]
    public async Task<IActionResult> Update(Guid id,CancellationToken ct)
    {
        var order = await orderApi.GetOrderAsync(id, ct);

        List<OrderItemViewModel> orderItemViewModels = [];
        foreach(var orderItem in order.OrderItems)
        {
            var orderItemVM = new OrderItemViewModel
            {
                PlantVariantId = orderItem.PlantVariantId,
                Id = orderItem.Id,
                ProductNameAtPurchase = orderItem.ProductNameAtPurchase,
                OrderId = orderItem.OrderId,
                Quantity = orderItem.Quantity,
                UnitPriceAtPurchase = orderItem.UnitPriceAtPurchase
            };
            orderItemViewModels.Add(orderItemVM);
        }

        var orderVm = new OrderDetailsViewModel
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            Status = order.Status,
            PaymentStatus = order.PaymentStatus,
            OrderItems = orderItemViewModels
        };
        return View(orderVm);
    }
}
