
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Catalog.Domain.Models;

public class PlantImageConfiguration : IEntityTypeConfiguration<PlantImage>
{
    public void Configure(EntityTypeBuilder<PlantImage> builder)
    {
        builder.ToTable("PlantImages");

        builder.HasKey(x => x.Id); 
        builder.Property(x => x.Id).ValueGeneratedNever();


        builder.Property(x => x.StorageKey)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.AltText)
            .HasMaxLength(250);

        builder.HasIndex(x => x.PlantId);
        builder.HasIndex(x => x.PlantVariantId);

        builder.Ignore(x => x.DomainEvents);
    }
} 