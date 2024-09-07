using DailyBalances.Core.Entities.Balances;

namespace DailyBalances.Core.Infrastructure.Repositories;

public interface IBalanceRepository
{
    Task<Balance?> Find(string merchantId, DateTime balanceDay);
}
