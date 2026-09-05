
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
        builder.Property(p => p.ImageUrl).HasMaxLength(500);

        builder.Ignore(p => p.CategoryIds);

        //builder.HasMany<PlantCategory>("_categories")
        //.WithOne()
        //.HasForeignKey(pc => pc.PlantId)
        //.OnDelete(DeleteBehavior.Cascade);

        builder.Navigation("_categories")
            .UsePropertyAccessMode(PropertyAccessMode.Field);


        builder.HasMany(p => p.Variants)
        .WithOne()
        .HasForeignKey(v => v.PlantId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(p => p.Variants)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
