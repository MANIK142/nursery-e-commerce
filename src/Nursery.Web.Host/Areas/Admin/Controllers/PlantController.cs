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
            return Json(new { data = cards });

        }


        
        [HttpGet]
        public async Task<IActionResult> Create( CancellationToken ct = default)
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

        [HttpGet]
        public async Task<IActionResult> Update(Guid Id,CancellationToken ct = default)
        {
            var categories = await _catalogApiClient.GetCatgoriesAsync(ct);
            if (Id != Guid.Empty && Id != null)
            {
            }


            var plant = await _catalogApiClient.GetPlantByIdAsync(Id, ct);
            var editPlantViewModel = new EditPlantViewModel
            {
                Id = plant.Id,
                AvailableCategories = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList(),
                Name = plant.Name,
                Description = plant.Description,
                SelectedCategories = plant.Categories.Select(c => c.Id.ToString()).ToList(),
                Images = plant.PlantImages.Select(pi => new PlantImageInputModel
                {
                    AltText = pi.AltText,
                    IsPrimaryImage = pi.IsPrimaryImage,
                    StorageKey = pi.StorageKey
                }).ToList(),
                PlantVariantSpecs = plant.PlantVariants.Select(pv => new PlantVariantInputModel
                {
                    Sku = pv.Sku,
                    RetailPrice = new MoneyInputModel
                    {
                        Amount = pv.RetailPrice.Amount,
                        Currency = pv.RetailPrice.Currency
                    },
                    WholesalePrice = new MoneyInputModel
                    {
                        Amount = pv.WholeSalePrice.Amount,
                        Currency = pv.WholeSalePrice.Currency
                    },
                    VariantName = pv.VariantName,
                    ImageSpecs = pv.Images.Select(i => new PlantImageInputModel
                    {
                        AltText = i.AltText,
                        IsPrimaryImage = i.IsPrimaryImage,
                        StorageKey = i.StorageKey
                    }).ToList()
                }).ToList(),
            };
            return View(editPlantViewModel);
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Guid id, EditPlantViewModel model, CancellationToken cancellationToken)
        {
            if (id != model.Id) return BadRequest();

            if (model.PlantVariantSpecs.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "At least one variant specification is required.");
            }

            model.Images.RemoveAll(img => string.IsNullOrWhiteSpace(img.StorageKey));
            foreach (var variant in model.PlantVariantSpecs)
            {
                variant.ImageSpecs.RemoveAll(img => string.IsNullOrWhiteSpace(img.StorageKey));
            }

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

            var payload = new UpdatePlantApiRequest(
                Id: model.Id,
                Name: model.Name,
                Description: model.Description ?? string.Empty,
                ModifiedBy: string.IsNullOrWhiteSpace(model.ModifiedBy) ? (User.Identity?.Name ?? "Admin") : model.ModifiedBy,
                Categories: model.SelectedCategories,
                PlantVariantSpecs: model.PlantVariantSpecs.Select(v => new PlantVariantSpecDto(
                    Sku: v.Sku,
                    VariantName: v.VariantName,
                    ImageSpecs: v.ImageSpecs.Select(i => new ImageSpecDto(i.StorageKey, i.IsPrimaryImage, i.AltText ?? string.Empty)).ToList(),
                    RetailPrice: new MoneyDto(v.RetailPrice.Amount, v.RetailPrice.Currency),
                    WholesalePrice: new MoneyDto(v.WholesalePrice.Amount, v.WholesalePrice.Currency)
                )).ToList(),
                Images: model.Images.Select(i => new ImageSpecDto(i.StorageKey, i.IsPrimaryImage, i.AltText ?? string.Empty)).ToList()
            );

            var (isSuccess, errorMessage) = await _catalogApiClient.UpdatePlantAsync(id, payload, cancellationToken);

            if (!isSuccess)
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "Failed to update plant.");
                var categories = await _catalogApiClient.GetCatgoriesAsync(cancellationToken);
                model.AvailableCategories = categories.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
                return View(model);
            }

            TempData["SuccessMessage"] = $"Plant \"{model.Name}\" was updated successfully!";
            return RedirectToAction(nameof(Update), new { id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePlantViewModel model, CancellationToken cancellationToken)
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

            var createdBy = User.Identity?.Name ?? "Admin";

            // 3. Map View Model to exact API DTO Payload
            var payload = new CreatePlantApiRequest(
                Name: model.Name,
                Description: model.Description ?? string.Empty,
                CreatedBy: createdBy,
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
            return RedirectToAction(nameof(Create));
        }

        [HttpDelete]
        public async Task<bool> Delete(Guid Id, CancellationToken cancellationToken)
        {

            var (IsSuccess, errorMessage) = await _catalogApiClient.DeletePlant(Id, cancellationToken);
            //if (IsSuccess)
            //{
            //    TempData["success"] = $"Plant \"{Id}\" was Deleted successfully!";
            //}
            return true;
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


        public async Task<IActionResult> CreateCareInstruction(Guid Id, CancellationToken cancellationToken)
        {

            var plant = await _catalogApiClient.GetPlantByIdAsync(Id,cancellationToken);
            if (plant.CareInstruction != null)
            {
                var updateCareInstructionViewModel = new CreateCareInstructionViewModel()
                {
                    Id = plant.CareInstruction.Id,
                    PlantId = Id,
                    Name = plant.Name,
                    AdditionalNotes = plant.CareInstruction.AdditionalNotes,
                    IsToxicToPets = plant.CareInstruction.IsToxicToPets,
                    SunlightRequirement = plant.CareInstruction.SunlightRequirement,
                    FertilizingFrequency = plant.CareInstruction.FertilizingFrequency,
                    HumidityLevel = plant.CareInstruction.HumidityLevel,
                    SoilType = plant.CareInstruction.SoilType,
                    WateringFrequency = plant.CareInstruction.WateringFrequency,    
                    DifficultyLevel = plant.CareInstruction.DifficultyLevel,
                    MaxTemperatureCelsius = plant.CareInstruction.MaxTemperatureCelsius,
                    MinTemperatureCelsius = plant.CareInstruction.MinTemperatureCelsius,
                    PruningNotes = plant.CareInstruction.PruningNotes
                };
                return View(updateCareInstructionViewModel);
            }
            var createCareInstructionViewModel = new CreateCareInstructionViewModel()
            {
                PlantId = Id,
                Name = plant.Name
            };
            return View(createCareInstructionViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCareInstruction(CreateCareInstructionViewModel CreateCareInstructionViewModel, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var createCareInstructionViewModel = new CreateCareInstructionViewModel()
                {
                    PlantId = CreateCareInstructionViewModel.PlantId,
                    Name = CreateCareInstructionViewModel.Name
                };
                return View(createCareInstructionViewModel);
            }

            var createdBy = User.Identity?.Name ?? "Admin";



            // 3. Map View Model to exact API DTO Payload

            var isSuccess = false;
            var errorMessage = "";
            if (CreateCareInstructionViewModel.Id != null)
            {
                var updatepayload = new UpdateCareInstructionRequest((Guid)CreateCareInstructionViewModel.Id, CreateCareInstructionViewModel.PlantId, CreateCareInstructionViewModel.WateringFrequency, CreateCareInstructionViewModel.SunlightRequirement,
                                                                CreateCareInstructionViewModel.SoilType, CreateCareInstructionViewModel.MinTemperatureCelsius, CreateCareInstructionViewModel.MaxTemperatureCelsius,
                                                                CreateCareInstructionViewModel.HumidityLevel, CreateCareInstructionViewModel.FertilizingFrequency, CreateCareInstructionViewModel.DifficultyLevel,
                                                                CreateCareInstructionViewModel.IsToxicToPets, CreateCareInstructionViewModel.PruningNotes, CreateCareInstructionViewModel.AdditionalNotes);
                 (isSuccess, errorMessage) = await _catalogApiClient.UpdateCareInstruction(updatepayload, cancellationToken);
            }
            else
            {
                var payload = new CreateCareInstructionRequest(CreateCareInstructionViewModel.PlantId, CreateCareInstructionViewModel.WateringFrequency, CreateCareInstructionViewModel.SunlightRequirement,
                                                                CreateCareInstructionViewModel.SoilType, CreateCareInstructionViewModel.MinTemperatureCelsius, CreateCareInstructionViewModel.MaxTemperatureCelsius,
                                                                CreateCareInstructionViewModel.HumidityLevel, CreateCareInstructionViewModel.FertilizingFrequency, CreateCareInstructionViewModel.DifficultyLevel,
                                                                CreateCareInstructionViewModel.IsToxicToPets, CreateCareInstructionViewModel.PruningNotes, CreateCareInstructionViewModel.AdditionalNotes);
                 (isSuccess, errorMessage) = await _catalogApiClient.CreateCareInstruction(payload, cancellationToken);
            }
            // 4. Send request to Backend API
            

            if (!isSuccess)
            {
                ModelState.AddModelError(string.Empty, errorMessage ?? "Failed to publish plant.");
                return RedirectToAction(nameof(CreateCareInstruction));
            }

            TempData["success"] = $"Plant Care Instruction  \"{CreateCareInstructionViewModel.Name}\" was successfully published!";
            return RedirectToAction(nameof(CreateCareInstruction));
        }
    }
}
