using Microsoft.AspNetCore.Mvc;
using Nursery.Web.Host.Services;
using Nursery.Web.Host.ViewModels;
using System.Numerics;

namespace Nursery.Web.Host.Areas.Customer.Controllers;

public class PlantsController : Controller
{
    private readonly ICatalogApiClient _catalogApi;
    private readonly string _imageBaseUrl;

    public PlantsController(ICatalogApiClient catalogApi, IConfiguration config)
    {
        _catalogApi = catalogApi;
        _imageBaseUrl = config["ImageBaseUrl"] ?? "";
    }

    public async Task<IActionResult> Index(int page = 1, CancellationToken ct = default)
    {
        var plants = await _catalogApi.GetPlantsAsync(ct);
        var cards = plants.Select(p => p.ToCardViewModel(_imageBaseUrl)).ToList();
        return View(cards);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct = default)
    {
        var plant = await _catalogApi.GetPlantByIdAsync(id, ct);
        if (plant is null) return NotFound();
        var card = plant.ToCardViewModel(_imageBaseUrl);
        return View(card);
    }
}