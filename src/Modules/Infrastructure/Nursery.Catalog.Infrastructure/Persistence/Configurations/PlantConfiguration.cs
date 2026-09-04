
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Catalog.Domain.Models;


namespace Nursery.Catalog.Infrastructure.Persistence.Configurations;

public class PlantConfiguration : IEntityTypeConfiguration<Plant>
{
    public void Configure(EntityTypeBuilder<Plant> builder)
    {
        builder.ToTable("Plants");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.SkuCode).IsUnique();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.SkuCode).IsRequired().HasMaxLength(15);
        builder.Property(p => p.RetailPrice).HasPrecision(18, 2);
        builder.Property(p => p.ImageUrl).HasMaxLength(500);

        builder.Ignore(p => p.CategoryIds);


        builder.Navigation("_categories")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
