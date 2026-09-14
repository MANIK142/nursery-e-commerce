
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Infrastructure.Persistence.Configurations;

public class VariantSalePriceConfiguration : IEntityTypeConfiguration<VariantSalePrice>
{
    public void Configure(EntityTypeBuilder<VariantSalePrice> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.OwnsOne(vsp => vsp.SalePrice, sp =>
        {
            sp.Property(p => p.Amount)
                .HasColumnName("SalePrice_Amount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            sp.Property(p => p.Currency)
                .HasColumnName("SalePrice_Currency")
                .HasMaxLength(5)
                .IsRequired();
        });

        builder.Ignore(x => x.DomainEvents);

    }
}
