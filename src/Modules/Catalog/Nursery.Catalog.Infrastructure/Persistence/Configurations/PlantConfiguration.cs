
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client.RP;
using Nursery.Catalog.Domain.Models;


namespace Nursery.Catalog.Infrastructure.Persistence.Configurations;

public class PlantConfiguration : IEntityTypeConfiguration<Plant>
{
    public void Configure(EntityTypeBuilder<Plant> builder)
    {
        builder.ToTable("Plants");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);

        builder.Ignore(p => p.CategoryIds);

        builder.HasMany(p => p.Categories)
            .WithOne()
            .HasForeignKey(pc => pc.PlantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Categories)
            .UsePropertyAccessMode(PropertyAccessMode.Field);


        //builder.Ignore(p => p.CategoryIds);
        //builder.Navigation("_categories")
        //    .UsePropertyAccessMode(PropertyAccessMode.Field);


        builder.HasMany(p => p.Variants)
        .WithOne()
        .HasForeignKey(v => v.PlantId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Variants)
            .UsePropertyAccessMode(PropertyAccessMode.Field);


        builder.HasMany(p => p.Images)
            .WithOne()
            .HasForeignKey(pi => pi.PlantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.CareInstruction)
            .WithOne()
            .HasForeignKey<CareInstruction>(ci => ci.PlantId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Ignore(x => x.DomainEvents);
    }
}
