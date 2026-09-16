using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nursery.Web.Host.Models.Catalog;
using Nursery.Web.Host.Services.Interface;
using Nursery.Web.Host.ViewModels;
using System.Numerics;

namespace Nursery.Web.Host.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PlantController : Controller
    {
        private readonly ICatalogApiClient _catalogApiClient;

        public PlantController(ICatalogApiClient catalogApiClient)
        {
            this._catalogApiClient = catalogApiClient;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {

            return View();
        }


        [AllowAnonymous]
        public async Task<IActionResult> getall(CancellationToken ct =default)
        {
            var plants = await _catalogApiClient.GetPlantsAsync(ct);
            var cards = plants.Select(p => p.ToCardViewModel("")).ToList();
            return View(cards);
        }

        public async Task<IActionResult> Upsert(int? Id, CancellationToken ct = default)
        {
            var categories = await _catalogApiClient.GetCatgoriesAsync(ct);

            var plantVm = new PlantVM
            {
                CategoryList = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }),
                Plant = default!
            };
            return View(plantVm);
        }
    }
}
