using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nursery.Web.Host.Models.Catalog;

namespace Nursery.Web.Host.ViewModels;

public class PlantVM
{
    public PlantDto Plant { get; set; }
    [ValidateNever]
    public IEnumerable<SelectListItem> CategoryList { get; set; } = [];
}
