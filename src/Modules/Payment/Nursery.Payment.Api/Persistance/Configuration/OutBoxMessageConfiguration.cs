using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Payment.Api.Models;

namespace Nursery.Payment.Api.Persistance.Configuration;

public class OutBoxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.ProcessedOnUtc);
    }
}
