
using Customer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Persistance.Configuration;

public class CustomerEntityConfiguration : IEntityTypeConfiguration<CustomerEntity>
{
    public void Configure(EntityTypeBuilder<CustomerEntity> builder)
    {
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).HasMaxLength(50);
        builder.Property(x => x.EmailAddress).IsRequired().HasMaxLength(100);
        builder.Property(x => x.ExternalUserId).IsRequired().HasMaxLength(200);
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);

        builder.HasMany(x=>x.Addresses)
            .WithOne()
            .HasForeignKey(x=>x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Addresses)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}
