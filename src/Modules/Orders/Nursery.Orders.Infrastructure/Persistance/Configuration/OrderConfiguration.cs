
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Orders.Domain.Orders;

namespace Nursery.Orders.Infrastructure.Persistance.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedNever();

        builder.Property(o=>o.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(o => o.PaymentStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(15);

        builder.HasMany(o => o.OrderItems)
          .WithOne()
          .HasForeignKey(oi => oi.OrderId)
          .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.OrderItems)
            .HasField("_orderItems")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.OwnsOne(o => o.ShippingAddress, sa =>
        {
            sa.Property(a => a.FirstName).HasMaxLength(100).HasColumnName("ShippingAddress_FirstName");
            sa.Property(a => a.LastName).HasMaxLength(100).HasColumnName("ShippingAddress_LastName");
            sa.Property(a => a.EmailAddress).HasMaxLength(256).HasColumnName("ShippingAddress_EmailAddress");
            sa.Property(a => a.AddressLine).IsRequired().HasMaxLength(200).HasColumnName("ShippingAddress_AddressLine");
            sa.Property(a => a.Country).IsRequired().HasMaxLength(100).HasColumnName("ShippingAddress_Country");
            sa.Property(a => a.State).IsRequired().HasMaxLength(100).HasColumnName("ShippingAddress_State");
            sa.Property(a => a.ZipCode).IsRequired().HasMaxLength(20).HasColumnName("ShippingAddress_ZipCode");
        });
        builder.Navigation(o => o.ShippingAddress).IsRequired();

        builder.OwnsOne(o => o.BillingAddress, ba =>
        {
            ba.Property(a => a.FirstName).HasMaxLength(100).HasColumnName("BillingAddress_FirstName");
            ba.Property(a => a.LastName).HasMaxLength(100).HasColumnName("BillingAddress_LastName");
            ba.Property(a => a.EmailAddress).HasMaxLength(256).HasColumnName("BillingAddress_EmailAddress");
            ba.Property(a => a.AddressLine).IsRequired().HasMaxLength(200).HasColumnName("BillingAddress_AddressLine");
            ba.Property(a => a.Country).IsRequired().HasMaxLength(100).HasColumnName("BillingAddress_Country");
            ba.Property(a => a.State).IsRequired().HasMaxLength(100).HasColumnName("BillingAddress_State");
            ba.Property(a => a.ZipCode).IsRequired().HasMaxLength(20).HasColumnName("BillingAddress_ZipCode");
        });

        
    }
}
