using Microsoft.AspNetCore.Mvc;
using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.ViewModels.Cart;
using Nursery.Web.Host.Models.ViewModels.Checkout;
using Nursery.Web.Host.Services.Interface;

namespace Nursery.Web.Host.Areas.Customer.Controllers;

[Area("Customer")]
public class CheckoutController(ICheckoutApi checkoutApi,ICatalogApiClient catalogApi,IOrderApiClient orderApi,ICurrentUserService currentUser) : Controller
{
    private readonly ICheckoutApi _checkoutApi = checkoutApi;

    private readonly ICatalogApiClient _catalogApi  = catalogApi;
    private readonly IOrderApiClient _orderApi = orderApi;
    public ICurrentUserService CurrentUser { get; } = currentUser;

    public async Task<IActionResult> Index(CancellationToken _cancellationToken)
    {

        var vm = await BuildViewModelAsync(_cancellationToken);
        if (vm is null) return RedirectToAction("Cart", "Plants");
        vm.IdempotencyKey = Guid.NewGuid();
        return View(vm);
    }
    private async Task<CheckoutViewModel?> BuildViewModelAsync(CancellationToken ct)
    {
        var model = new CheckoutViewModel();
        model.CustomerId = CurrentUser.CustomerId;

        var customerAddress = await _checkoutApi.GetAddressesAsync(ct);

        model.Addresses = customerAddress;
        var cart = await _orderApi.GetCartAsync(ct);
        List<Guid> plantVariantIds = cart.CartItemDtos.Select(ci => ci.PlantVariantId).ToList();
        if (plantVariantIds.Any())
        {
            var request = new GetPlantVariantsRequest(plantVariantIds);
            var plantVariants = await _catalogApi.GetPlantVariantsAsync(request, ct);
            if (plantVariants.PlantVariants.Any())
            {

                foreach (var cartItem in cart.CartItemDtos)
                {
                    var plantVariant = plantVariants.PlantVariants.FirstOrDefault(pv => pv.Id == cartItem.PlantVariantId);

                    var checkoutViewModel = new CheckoutItemViewModel
                    {
                        PlantVariantId = cartItem.PlantVariantId,
                        Quantity = cartItem.Quantity,
                        Sku = plantVariant.Sku,
                        VariantName = plantVariant.VariantName,
                        PlantName = plantVariant.VariantName,
                        UnitPrice = plantVariant.Price.Amount,
                        ImageUrl = plantVariant.Images.FirstOrDefault().StorageKey
                    };
                    model.Items.Add(checkoutViewModel);

                }

            }

        }
        return model;
    }
}
