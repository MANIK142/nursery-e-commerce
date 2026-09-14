
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Shippings.Domain.Models;

namespace Nursery.Shippings.Infrastucture.Presistance.Configuration;

public class ReturnLineItemConfiguration : IEntityTypeConfiguration<ReturnLineItem>
{
    public void Configure(EntityTypeBuilder<ReturnLineItem> builder)
    {
        builder.ToTable("ReturnLineItems", "shipping");

        builder.HasKey(li => li.Id);
        builder.Property(li => li.Id).ValueGeneratedNever();

        builder.Property(li => li.ShipmentLineItemId)
            .IsRequired();

        builder.Property(li => li.Quantity)
            .IsRequired();

        builder.HasIndex(li => li.ShipmentLineItemId);
    }
}
