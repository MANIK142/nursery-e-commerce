
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Shippings.Domain.Models;

namespace Nursery.Shippings.Infrastucture.Presistance.Configuration;

public class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipments", "shipping");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(s => s.OrderId)
            .IsRequired();

        builder.Property(s => s.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(s => s.CreatedAt).IsRequired();
        builder.Property(s => s.ShippedAt);
        builder.Property(s => s.DeliveredAt);

        builder.OwnsOne(s => s.TrackingInfo, tb =>
        {
            tb.Property(t => t.Carrier)
                .HasColumnName("Carrier")
                .HasMaxLength(100);

            tb.Property(t => t.TrackingNumber)
                .HasColumnName("TrackingNumber")
                .HasMaxLength(100);

            tb.Property(t => t.TrackingUrl)
                .HasColumnName("TrackingUrl")
                .HasMaxLength(500);
        });

        builder.HasMany(s => s.LineItems)
            .WithOne()
            .HasForeignKey("ShipmentId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(s => s.LineItems)
            .HasField("_lineItems")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Supports the read-side "sum shipped quantity per order" lookup
        // that IOrderLineItemLookup's Shipping-side consumer will need.
        builder.HasIndex(s => s.OrderId);
    }
}
