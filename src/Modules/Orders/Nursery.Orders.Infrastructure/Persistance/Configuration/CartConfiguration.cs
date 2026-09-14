

using Microsoft.EntityFrameworkCore;
using Nursery.Orders.Domain.Carts;

namespace Nursery.Orders.Infrastructure.Persistance.Configuration;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c=> c.Id).ValueGeneratedNever();



        builder.Property(c => c.CartStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(c => c.CustomerId).IsRequired();

        builder.HasMany(c => c.Items)
        .WithOne()
        .HasForeignKey(ci => ci.CartId)
        .OnDelete(DeleteBehavior.Cascade);


        builder.Navigation(x => x.Items)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        builder.Ignore(x => x.DomainEvents);

    }
}
