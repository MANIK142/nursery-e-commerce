

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Infrastructure.Persistence.Configurations;

public class PlantVariantConfiguration : IEntityTypeConfiguration<PlantVariant>
{
    public void Configure(EntityTypeBuilder<PlantVariant> builder)
    {

        builder.HasKey(pv => pv.Id);
        builder.Property(pv => pv.Id).ValueGeneratedNever();

        builder.Property(pv => pv.Sku).IsRequired().HasMaxLength(20);
        builder.Property(pv => pv.VariantName).IsRequired().HasMaxLength(100);

        builder.OwnsOne(pv => pv.RetailPrice, m =>
        {
            m.Property(x => x.Amount).HasColumnName("RetailPrice_Amount").HasColumnType("decimal(18,2)").IsRequired();
            m.Property(x => x.Currency).HasColumnName("RetailPrice_Currency").HasMaxLength(3);
        });
        builder.OwnsOne(pv => pv.WholesalePrice, m =>
        {
            m.Property(x => x.Amount).HasColumnName("WholesalePrice_Amount").HasColumnType("decimal(18,2)").IsRequired();
            m.Property(x => x.Currency).HasColumnName("WholesalePrice_Currency").HasMaxLength(3);
        });

        builder.HasMany(pv => pv.SalePrices)
            .WithOne()
            .HasForeignKey(v => v.PlantVariantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(v => v.SalePrices)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(pv => pv.Images)
            .WithOne()
            .HasForeignKey(pi => pi.PlantVariantId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
