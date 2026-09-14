
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Catalog.Domain.Models;

namespace Nursery.Catalog.Infrastructure.Persistence.Configurations;

public class CareInstructionConfiguration : IEntityTypeConfiguration<CareInstruction>
{
    public void Configure(EntityTypeBuilder<CareInstruction> builder)
    {
        builder.ToTable("CareInstructions");
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.PlantId).IsUnique();

        builder.Property(c => c.WateringFrequency)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.SunlightRequirement)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.SoilType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.HumidityLevel)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(c => c.FertilizingFrequency)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.DifficultyLevel)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(c => c.PruningNotes)
            .HasMaxLength(1000);

        builder.Property(c => c.AdditionalNotes)
            .HasMaxLength(1000);

        builder.Ignore(x => x.DomainEvents);
    }
}
