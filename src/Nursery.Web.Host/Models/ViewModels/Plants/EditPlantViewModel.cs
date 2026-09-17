namespace Nursery.Web.Host.Models.ViewModels.Plants;

public class EditPlantViewModel : CreatePlantViewModel
{
    public Guid Id { get; set; } 
    public string ModifiedBy { get; set; } = string.Empty;
}
