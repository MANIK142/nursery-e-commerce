using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nursery.Payment.Api.Models;

namespace Nursery.Payment.Api.Persistance.Configuration;

public class PaymentAttemptConfiguration : IEntityTypeConfiguration<PaymentAttempt>
{
    public void Configure(EntityTypeBuilder<PaymentAttempt> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.HasIndex(x => x.PaymentId);

        builder.Property(x => x.GatewayProvider).IsRequired().HasMaxLength(50);
        builder.Property(x => x.GatewayPaymentIntentId).IsRequired().HasMaxLength(200);

        builder.Property(x => x.AttemptStatus).IsRequired().HasMaxLength(30).HasConversion<string>();
        builder.Property(x => x.FailureReason).HasMaxLength(1000);

        builder.Ignore(x => x.DomainEvents);
    }
}
