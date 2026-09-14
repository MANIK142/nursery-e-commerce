using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Nursery.Shippings.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Shippings.Infrastucture.Presistance.Configuration;

internal class ShipmentLineItemConfiguration : IEntityTypeConfiguration<ShipmentLineItem>
{
    public void Configure(EntityTypeBuilder<ShipmentLineItem> builder)
    {
        builder.ToTable("ShipmentLineItems", "shipping");
        builder.HasKey(li => li.Id);
        builder.Property(li => li.Id).ValueGeneratedNever();

        builder.Property(li => li.OrderItemId).IsRequired();
        builder.Property(li => li.ProductId).IsRequired();
        builder.Property(li => li.Quantity).IsRequired();

        // Needed by Return: ReturnLineItem.ShipmentLineItemId points here.
        // No FK constraint across aggregates (Return is a separate aggregate root),
        // just an index for the lookup query.
        builder.HasIndex(li => li.OrderItemId);
    }
}
