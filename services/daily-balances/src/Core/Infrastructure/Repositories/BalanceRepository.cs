using DailyBalances.Core.Entities.Balances;
using DailyBalances.Core.Infrastructure.Data;

namespace DailyBalances.Core.Infrastructure.Repositories;

public sealed class BalanceRepository : IBalanceRepository
{
    private readonly BalancesDbContext context;

    public BalanceRepository(
        BalancesDbContext context)
    {
        this.context = context;
    }

    public async Task<Balance?> Find(string merchantId, DateTime balanceDay)
    {
        return await context.Set<Balance>().FindAsync(merchantId, balanceDay);
    }
}
