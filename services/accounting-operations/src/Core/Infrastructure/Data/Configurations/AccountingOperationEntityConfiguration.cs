using AccountingOperations.Core.Entities.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingOperations.Core.Infrastructure.Data.Configuration;

public sealed class AccountingOperationEntityConfiguration
    : IEntityTypeConfiguration<AccountingOperation>
{
    public void Configure(EntityTypeBuilder<AccountingOperation> builder)
    {
        builder.ToTable("operations", options => options.ExcludeFromMigrations());

        builder.HasKey(p => new { p.MerchantId, p.RegistrationDate });

        builder
            .Property(p => p.MerchantId)
            .HasColumnName("merchant_id");

        builder
            .Property(p => p.RegistrationDate)
            .HasColumnName("registered_at")
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder
            .Property(p => p.Value)
            .HasColumnName("value");

        builder
            .Property(p => p.Type)
            .HasColumnName("type");
    }
}
