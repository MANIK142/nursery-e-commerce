using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nursery.Web.Host.Models.DTOs;
using Nursery.Web.Host.Models.ViewModels;
using Nursery.Web.Host.Models.ViewModels.Cart;
using Nursery.Web.Host.Services.Interface;
using System.Numerics;

namespace Nursery.Web.Host.Areas.Customer.Controllers;

[Area("Customer")]
//[Authorize(Roles = "Customer")]
public class PlantsController : Controller
{
    private readonly ICatalogApiClient _catalogApi;
    private readonly IOrderApiClient _OrderApi;
    private readonly string _imageBaseUrl;

    public PlantsController(ICatalogApiClient catalogApi, IOrderApiClient orderApi, IConfiguration config)
    {
        _catalogApi = catalogApi;
        _imageBaseUrl = config["ImageBaseUrl"] ?? "";
        _OrderApi = orderApi;
    }


    [AllowAnonymous]
    public async Task<IActionResult> Index(int page = 1, CancellationToken ct = default)
    {
        var plants = await _catalogApi.GetPlantsAsync(ct);
        var cards = plants.Select(p => p.ToCardViewModel(_imageBaseUrl)).ToList();
        return View(cards);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id, CancellationToken ct = default)
    {
        var plant = await _catalogApi.GetPlantByIdAsync(id, ct);
        if (plant is null) return NotFound();

        ViewData["baseUrl"] = _imageBaseUrl;
        return View(plant);
    }


    [HttpPost("/customer/plants/addtocart")]
    public async Task<IActionResult> AddToCart(Guid plantVariantId, CancellationToken ct = default)
    {
        if (plantVariantId == Guid.Empty)
        {
            return BadRequest(new { success = false, message = "Invalid plant variant selected." });
        }

        try
        {
            // Pass quantity if your API supports it: AddToCardAsync(plantVariantId, quantity, ct)
            await _OrderApi.AddToCardAsync(plantVariantId, ct);
            return Ok(new { success = true, message = "Added to cart successfully!" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Failed to add to cart." });
        }
    }


    [HttpGet("/customer/plants/getcartcount")]
    public async Task<IActionResult> GetCartCount(CancellationToken ct = default)
    {
        var count = await _OrderApi.GetCartCount(ct);
        return Ok(count);
    }

    [HttpGet("/customer/plants/getcart")]
    public async Task<IActionResult> Cart(CancellationToken ct = default)
    {
        var cartViewModel = new CartViewModel()
        {
            Items = []
        };
        var cart =  await _OrderApi.GetCartAsync(ct);
        if(cart != null)
        {
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

                        var cartItemViewModel = new CartItemsViewModel
                        {
                            PlantVariantId = cartItem.PlantVariantId,
                            Price = cartItem.Price,
                            Quantity = cartItem.Quantity,
                            PlantVariantName = plantVariant.VariantName,
                            Sku = plantVariant.Sku
                        };
                        cartViewModel.Items.Add(cartItemViewModel);

                    }

                }

            }
        }
        

        return View(cartViewModel);
    }



    [ValidateAntiForgeryToken]
    public async Task<IActionResult> IncreaseQuantity(Guid plantVariantId, CancellationToken ct)
    {

        try
        {
            // Pass quantity if your API supports it: AddToCardAsync(plantVariantId, quantity, ct)
            await _OrderApi.IncreaseCartItem(plantVariantId, ct);
            //return Ok(new { success = true, message = "Added to cart successfully!" });
            return RedirectToAction(nameof(Cart));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Failed to add to cart." });
        }
        
    }



    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeccreaseQuantity(Guid plantVariantId, CancellationToken ct)
    {

        try
        {
            // Pass quantity if your API supports it: AddToCardAsync(plantVariantId, quantity, ct)
            await _OrderApi.DecreaseCartItem(plantVariantId, ct);
            //return Ok(new { success = true, message = "Added to cart successfully!" });
            return RedirectToAction(nameof(Cart));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Failed to add to cart." });
        }
    }


    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromCart(Guid plantVariantId, CancellationToken ct)
    {
        try
        {
            // Pass quantity if your API supports it: AddToCardAsync(plantVariantId, quantity, ct)
            await _OrderApi.DeleteCartItem(plantVariantId, ct);
            //return Ok(new { success = true, message = "Added to cart successfully!" });
            return RedirectToAction(nameof(Cart));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = "Failed to add to cart." });
        }
    }


}