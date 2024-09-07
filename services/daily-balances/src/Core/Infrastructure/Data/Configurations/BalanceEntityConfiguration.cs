using DailyBalances.Core.Entities.Balances;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DailyBalances.Core.Infrastructure.Data.Configuration;

public sealed class BalanceEntityConfiguration
    : IEntityTypeConfiguration<Balance>
{
    public void Configure(EntityTypeBuilder<Balance> builder)
    {
        builder.ToTable("balances", options => options.ExcludeFromMigrations());

        builder.HasKey(p => new { p.MerchantId, p.Day });

        builder
            .Property(p => p.MerchantId)
            .HasColumnName("merchant_id");

        builder
            .Property(p => p.Day)
            .HasColumnName("day")
            .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder
            .Property(p => p.Total)
            .HasColumnName("total");
    }
}
