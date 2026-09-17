namespace Nursery.Web.Host.Models.DTOs;
public record CreatePlantApiRequest(
    string Name,
    string Description,
    string CreatedBy,
    List<string> Categories,
    List<PlantVariantSpecDto> PlantVariantSpecs,
    List<ImageSpecDto> Images
);
public record PlantVariantSpecDto(
    string Sku,
    string VariantName,
    List<ImageSpecDto> ImageSpecs,
    MoneyDto RetailPrice,
    MoneyDto WholesalePrice
);
public record ImageSpecDto(
    string StorageKey,
    bool IsPrimaryImage,
    string AltText
);
public record MoneyDto(
    decimal Amount,
    string Currency
);