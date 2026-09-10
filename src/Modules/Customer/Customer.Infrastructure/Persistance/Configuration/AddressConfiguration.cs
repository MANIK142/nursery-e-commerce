using Customer.Domain.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Persistance.Configuration;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.Line1).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Line2).HasMaxLength(200);
        builder.Property(x => x.City).IsRequired().HasMaxLength(50);
        builder.Property(x => x.State).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PostalCode).IsRequired().HasMaxLength(10);
        builder.Property(x => x.Country).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Type).HasConversion<string>().IsRequired().HasMaxLength(10);

    }
}
