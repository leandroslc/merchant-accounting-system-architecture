using Microsoft.EntityFrameworkCore;

namespace DailyBalances.Core.Infrastructure.Data;

public sealed class BalancesDbContext : DbContext
{
    public BalancesDbContext(DbContextOptions<BalancesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BalancesDbContext).Assembly);
    }
}
