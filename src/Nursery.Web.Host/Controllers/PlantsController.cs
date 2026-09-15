using Microsoft.AspNetCore.Mvc;
using Nursery.Web.Host.Services;

namespace Nursery.Web.Host.Controllers;

public class PlantsController : Controller
{
    private readonly ICatalogApiClient _catalogApi;

    public PlantsController(ICatalogApiClient catalogApi) => _catalogApi = catalogApi;

    public async Task<IActionResult> Index(int page = 1, CancellationToken ct = default)
    {
        var result = await _catalogApi.GetPlantsAsync(page, ct: ct);
        return View(result);
    }

    public async Task<IActionResult> Details(Guid id, CancellationToken ct = default)
    {
        var plant = await _catalogApi.GetPlantByIdAsync(id, ct);
        if (plant is null)
            return NotFound();

        return View(plant);
    }
}