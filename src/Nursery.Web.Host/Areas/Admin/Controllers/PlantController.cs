using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nursery.Web.Host.Models.DTOs;
//using Nursery.Web.Host.Models.DTOs.Catalog;
using Nursery.Web.Host.Models.ViewModels;
using Nursery.Web.Host.Models.ViewModels.Plants;
using Nursery.Web.Host.Services.Interface;
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
            var CreatePlantViewModel = new CreatePlantViewModel
            {
              AvailableCategories = categories.Select(c => new SelectListItem
                                                        {
                                                            Text = c.Name,
                                                            Value = c.Id.ToString()
                                                        }).ToList(),
              Name= "",
              Description ="",
              SelectedCategories = [],
              Images = new (),
              PlantVariantSpecs = new(),
            };
            return View(CreatePlantViewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CreatePlantViewModel model, CancellationToken cancellationToken)
        {
            // 1. Business & Cross-field validations
            if (model.PlantVariantSpecs.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "At least one variant specification is required.");
            }

            // Clean out blank uploaded keys (in case user added an image container but didn't upload)
            model.Images.RemoveAll(img => string.IsNullOrWhiteSpace(img.StorageKey));
            foreach (var variant in model.PlantVariantSpecs)
            {
                variant.ImageSpecs.RemoveAll(img => string.IsNullOrWhiteSpace(img.StorageKey));
            }

            // 2. Return on validation error without losing data
            if (!ModelState.IsValid)
            {
                var categories = await _catalogApiClient.GetCatgoriesAsync(cancellationToken);
                model.AvailableCategories = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
                return View(model);
            }

            // 3. Map View Model to exact API DTO Payload
            var payload = new CreatePlantApiRequest(
                Name: model.Name,
                Description: model.Description ?? string.Empty,
                CreatedBy: string.IsNullOrWhiteSpace(model.CreatedBy) ? (User.Identity?.Name ?? "Admin") : model.CreatedBy,
                Categories: model.SelectedCategories,
                PlantVariantSpecs: model.PlantVariantSpecs.Select(v => new PlantVariantSpecDto(
                    Sku: v.Sku,
                    VariantName: v.VariantName,
                    ImageSpecs: v.ImageSpecs.Select(img => new ImageSpecDto(
                        StorageKey: img.StorageKey,
                        IsPrimaryImage: img.IsPrimaryImage,
                        AltText: img.AltText ?? string.Empty
                    )).ToList(),
                    RetailPrice: new MoneyDto(v.RetailPrice.Amount, v.RetailPrice.Currency),
                    WholesalePrice: new MoneyDto(v.WholesalePrice.Amount, v.WholesalePrice.Currency)
                )).ToList(),
                Images: model.Images.Select(img => new ImageSpecDto(
                    StorageKey: img.StorageKey,
                    IsPrimaryImage: img.IsPrimaryImage,
                    AltText: img.AltText ?? string.Empty
                )).ToList()
            );

            // 4. Send request to Backend API
            var (isSuccess, errorMessage) = await _catalogApiClient.CreatePlantAsync(payload, cancellationToken);

            if (!isSuccess)
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "Failed to publish plant.");
                var categories = await _catalogApiClient.GetCatgoriesAsync(cancellationToken);
                model.AvailableCategories = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
                return View(model);
            }

            TempData["success"] = $"Plant \"{model.Name}\" was successfully published!";
            return RedirectToAction(nameof(Upsert));
        }

        [HttpPost("/admin/plants/upload-image")]
        public async Task<IActionResult> ProxyImageUpload([FromServices] IHttpClientFactory httpClientFactory)
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            var client = httpClientFactory.CreateClient();

            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream();
            using var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            content.Add(streamContent, "file", file.FileName);

            var apiResponse = await client.PostAsync("https://localhost:7139/api/v1/images/upload", content);
            var responseBody = await apiResponse.Content.ReadAsStringAsync();

            return StatusCode((int)apiResponse.StatusCode, responseBody);
        }
    }
}
