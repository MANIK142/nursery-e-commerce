
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Orders.Domain.Orders;

namespace Nursery.Orders.Infrastructure.Persistance.Configuration;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    void IEntityTypeConfiguration<OrderItem>.Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);
        builder.Property(oi => oi.Id).ValueGeneratedNever();

        builder.Property(oi => oi.OrderId)
            .IsRequired();

        builder.Property(oi => oi.PlantVariantId)
            .IsRequired();

        builder.Property(oi => oi.ProductNameAtPurchase)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.UnitPriceAtPurchase)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Ignore(oi => oi.LineTotal);

        builder.HasIndex(oi => oi.OrderId);

        builder.Ignore(x => x.DomainEvents);
    }
}
