

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Infrastructure.Persistence.Configurations;

public class PlantVariantConfiguration : IEntityTypeConfiguration<PlantVariant>
{
    public void Configure(EntityTypeBuilder<PlantVariant> builder)
    {
        builder.Property(pv => pv.Sku).IsRequired().HasMaxLength(20);
        builder.Property(pv => pv.VariantName).IsRequired().HasMaxLength(100);

        builder.HasMany<VariantSalePrice>()
            .WithOne()
            .HasForeignKey(v => v.PlantVariantId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}
