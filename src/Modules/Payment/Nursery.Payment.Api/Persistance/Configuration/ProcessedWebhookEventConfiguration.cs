using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Payment.Api.Models;

namespace Nursery.Payment.Api.Persistance.Configuration
{
    public class ProcessedWebhookEventConfiguration : IEntityTypeConfiguration<ProcessedWebhookEvent>
    {
        public void Configure(EntityTypeBuilder<ProcessedWebhookEvent> builder)
        {

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.EventId)
                .HasMaxLength(255)
                .IsRequired();

            builder.HasIndex(e => e.EventId).IsUnique();

            builder.Ignore(x => x.DomainEvents);
        }
    }
}
