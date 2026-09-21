using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.DTOs.Customer;
using Nursery.Web.Host.Models.DTOs.Orders;
using Nursery.Web.Host.Models.ViewModels;
using Nursery.Web.Host.Models.ViewModels.Cart;
using Nursery.Web.Host.Models.ViewModels.Checkout;
using Nursery.Web.Host.Services.Interface;

namespace Nursery.Web.Host.Areas.Customer.Controllers;

[Area("Customer")]
public class CheckoutController(IConfiguration configuration, ICheckoutApi checkoutApi,
    ICatalogApiClient catalogApi,IOrderApiClient orderApi,ICurrentUserService currentUser,
    IOptions<StripeOptions> stripeOptions) : Controller
{
    private readonly ICheckoutApi _checkoutApi = checkoutApi;
    private readonly ICatalogApiClient _catalogApi  = catalogApi;
    private readonly IOrderApiClient _orderApi = orderApi;
    public ICurrentUserService CurrentUser { get; } = currentUser;

    private readonly string _stripePublishableKey = configuration["Stripe:PublishableKey"].ToString();

    public string ImageBaseUrl = configuration["ImageBaseUrl"].ToString();

    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken _cancellationToken)
    {

        var vm = await BuildViewModelAsync(_cancellationToken);
        if (vm is null) return RedirectToAction("Cart", "Plants");
        vm.IdempotencyKey = Guid.NewGuid();
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(CheckoutViewModel checkoutViewModel, CancellationToken _cancellationToken)
    {

        OrderAddressDto BillingAddress = new OrderAddressDto(checkoutViewModel.BillingAddress.FirstName, checkoutViewModel.BillingAddress.LastName,
                                                            checkoutViewModel.BillingAddress.EmailAddress, checkoutViewModel.BillingAddress.AddressLine,
                                                            checkoutViewModel.BillingAddress.Country, checkoutViewModel.BillingAddress.State, 
                                                            checkoutViewModel.BillingAddress.ZipCode);

        //if (checkoutViewModel.SameAsBilling)
        //{
        OrderAddressDto ShippingAddress = new OrderAddressDto(checkoutViewModel.BillingAddress.FirstName, checkoutViewModel.BillingAddress.LastName,
                                                        checkoutViewModel.BillingAddress.EmailAddress, checkoutViewModel.BillingAddress.AddressLine,
                                                        checkoutViewModel.BillingAddress.Country, checkoutViewModel.BillingAddress.State,
                                                        checkoutViewModel.BillingAddress.ZipCode);
        //}
        List<OrderItemDto> orderItems = [];
        foreach(var orderItem in checkoutViewModel.Items)
        {
            OrderItemDto orderItemDto = new OrderItemDto(orderItem.PlantVariantId, orderItem.Quantity);
            orderItems.Add(orderItemDto);
        }

        CreateOrderApiRequest createOrderRequest = new CreateOrderApiRequest(checkoutViewModel.CustomerId, BillingAddress, ShippingAddress, orderItems);
        var result = await orderApi.CreateOrder(createOrderRequest, _cancellationToken);

        return RedirectToAction("Pay", "Checkout", new { orderId = result.OrderId });
    }


    [HttpGet]
    public async Task<IActionResult> Pay(Guid orderId, CancellationToken ct)
    {
        var order = await orderApi.GetOrderAsync(orderId, ct);
        if (order is null) return NotFound();

        // Already paid, cancelled, etc. -> the confirmation page knows how to show every state.
        if (order.Status is not (OrderStatuses.PendingPayment or OrderStatuses.PaymentFailed))
            return RedirectToAction(nameof(Confirmation), new { orderId });

        var vm = new PayViewModel
        {
            Order = order,
            PublishableKey = _stripePublishableKey,
            ReturnUrl = Url.Action(nameof(Confirmation), "Checkout", new { orderId }, Request.Scheme)!
        };

        // NOTE: the Payments handler should be idempotent per order (reuse the open PaymentIntent),
        // otherwise every refresh of this page creates a new one.
        var payment = await orderApi.InitiatePaymentAsync(orderId, ct);
        if (payment.IsSuccess) vm.ClientSecret = payment.Response?.ClientSecret;
        else vm.Error = "Error";

        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Confirmation(Guid orderId, CancellationToken ct)
    {
        var order = await orderApi.GetOrderAsync(orderId, ct);
        if (order is null) return NotFound();

        return View(new ConfirmationViewModel { Order = order });
    }

    [HttpGet]
    public async Task<IActionResult> Status(Guid orderId, CancellationToken ct)
    {
        var order = await orderApi.GetOrderAsync(orderId, ct);
        return order is null ? NotFound() : Json(new { status = order.Status });
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
                        ImageUrl = $"{ImageBaseUrl}{plantVariant.Images.FirstOrDefault().StorageKey}"
                    };
                    model.Items.Add(checkoutViewModel);

                }

            }

        }
        return model;
    }
}
