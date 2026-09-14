

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Shippings.Domain.Models;

namespace Nursery.Shippings.Infrastucture.Presistance.Configuration;

public class ReturnConfiguration : IEntityTypeConfiguration<Return>
{
    public void Configure(EntityTypeBuilder<Return> builder)
    {
        builder.ToTable("Returns", "shipping");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedNever();

        builder.Property(r => r.ShipmentId)
            .IsRequired();

        builder.Property(r => r.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(r => r.Reason)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(r => r.RequestedAt)
            .IsRequired();

        builder.HasMany(r => r.LineItems)
            .WithOne()
            .HasForeignKey("ReturnId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(r => r.LineItems)
            .HasField("_lineItems")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        // Supports the "one active return per Shipment" invariant check —
        // Application-layer handler queries WHERE ShipmentId = @id AND Status
        // NOT IN (Rejected, Closed) before allowing Return.Create.
        builder.HasIndex(r => r.ShipmentId);
    }
}
