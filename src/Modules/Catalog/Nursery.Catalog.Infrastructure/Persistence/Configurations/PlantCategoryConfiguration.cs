using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Catalog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nursery.Catalog.Infrastructure.Persistence.Configurations;

public class PlantCategoryConfiguration : IEntityTypeConfiguration<PlantCategory>
{
    public void Configure(EntityTypeBuilder<PlantCategory> builder)
    {
        builder.HasKey(pc => new { pc.PlantId, pc.CategoryId });


        builder.HasOne<Plant>()
            .WithMany("_categories")
            .HasForeignKey(pc => pc.PlantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(pc => pc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

       
    }
}
