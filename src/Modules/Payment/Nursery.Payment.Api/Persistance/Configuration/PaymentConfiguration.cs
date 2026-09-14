using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Payment.Api.Models;

namespace Nursery.Payment.Api.Persistance.Configuration;

public class PaymentConfiguration : IEntityTypeConfiguration<Models.Payment>
{
    public void Configure(EntityTypeBuilder<Models.Payment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Amount).IsRequired().HasPrecision(18, 2);
        builder.Property(x => x.Currency).IsRequired().HasMaxLength(3);

        builder.Property(x => x.Status).IsRequired().HasConversion<string>().HasMaxLength(30);

        builder.HasMany(x => x.Attempts)
            .WithOne()
            .HasForeignKey(pa => pa.PaymentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Attempts)
            .UsePropertyAccessMode(PropertyAccessMode.Field);


        builder.Ignore(x => x.DomainEvents);
    }
}
