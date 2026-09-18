using Microsoft.AspNetCore.Mvc.Rendering;
using Nursery.Web.Host.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Nursery.Web.Host.Models.ViewModels.Plants;

public class CreateCareInstructionViewModel
{
    public Guid? Id { get; set; }
    [Required(ErrorMessage = "Plant Id is required.")]
    public Guid PlantId { get;  set; }

    public string Name { get; set; } = string.Empty;
    public WateringFrequency WateringFrequency { get;  set; }
    public List<SelectListItem> AvailableWateringFrequency { get; set; } = Enum.GetValues<WateringFrequency>().Select(e => new SelectListItem
                                                                                                                                            {
                                                                                                                                                Text = e.ToString(),
                                                                                                                                                Value = e.ToString()
                                                                                                                                            }).ToList();
    public SunlightRequirement SunlightRequirement { get;  set; }
    public List<SelectListItem> AvailableSunlightRequirement { get; set; } = Enum.GetValues<SunlightRequirement>().Select(e => new SelectListItem
                                                                                                                                                {
                                                                                                                                                    Text = e.ToString(),
                                                                                                                                                    Value = e.ToString()
                                                                                                                                                }).ToList();
    public SoilType SoilType { get;  set; }
    public List<SelectListItem> AvailableSoilType { get; set; } = Enum.GetValues<SoilType>().Select(e => new SelectListItem
    {
        Text = e.ToString(),
        Value = e.ToString()
    }).ToList();
    public int MinTemperatureCelsius { get;  set; }
    public int MaxTemperatureCelsius { get;  set; }
    public HumidityLevel HumidityLevel { get;  set; }
    public List<SelectListItem> AvailableHumidityLevel { get; set; } = Enum.GetValues<HumidityLevel>().Select(e => new SelectListItem
    {
        Text = e.ToString(),
        Value = e.ToString()
    }).ToList();
    public FertilizingFrequency FertilizingFrequency { get;  set; }
    public List<SelectListItem> AvailableFertilizingFrequency { get; set; } = Enum.GetValues<FertilizingFrequency>().Select(e => new SelectListItem
                                                                                                                                                {
                                                                                                                                                    Text = e.ToString(),
                                                                                                                                                    Value = e.ToString()
                                                                                                                                                }).ToList();
    public CareDifficultyLevel DifficultyLevel { get;  set; }
    public List<SelectListItem> AvailableCareDifficultyLevel { get; set; } = Enum.GetValues<CareDifficultyLevel>().Select(e => new SelectListItem
                                                                                                                                                {
                                                                                                                                                    Text = e.ToString(),
                                                                                                                                                    Value = e.ToString()
                                                                                                                                                }).ToList();
    public bool IsToxicToPets { get;  set; }

    public string? PruningNotes { get;  set; }
    public string? AdditionalNotes { get;  set; }
}
