using Nursery.Web.Host.Models.DTOs.Catalog;

namespace Nursery.Web.Host.Models.ViewModels;

public record PlantCardViewModel(
    Guid Id,
    string Name,
    string? ImageUrl,
    decimal DisplayPrice,
    decimal? SalePrice,
    string Currency,
    string Categories
);

public static class PlantMappingExtensions
{
    public static PlantCardViewModel ToCardViewModel(this PlantDto plant, string imageBaseUrl)
    {
        var primaryVariant = plant.PlantVariants.FirstOrDefault()
            ?? plant.PlantVariants.FirstOrDefault();

        var activeSale = primaryVariant?.Price;

        var primaryImage = plant.PlantImages.FirstOrDefault(i => i.IsPrimaryImage)
            ?? plant.PlantImages.FirstOrDefault();

        return new PlantCardViewModel(
            Id: plant.Id,
            Name: plant.Name,
            ImageUrl: primaryImage is not null ? $"{imageBaseUrl.TrimEnd('/')}/{primaryImage.StorageKey}" : null,
            DisplayPrice: primaryVariant?.RetailPrice.Amount ?? 0,
            SalePrice: activeSale?.Amount,
            Currency: primaryVariant?.RetailPrice.Currency ?? "INR",
            Categories: string.Join(", ", plant.Categories.Select(c => c.Name))
        );
    }


}