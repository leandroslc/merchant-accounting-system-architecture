using DailyBalances.Core.Entities.Balances;

namespace DailyBalances.Core.Infrastructure.Repositories;

public interface IBalanceRepository
{
    Task<Balance?> FindAsync(string merchantId, DateTime balanceDay);

    Task CreateOrUpdateAsync(string merchantId, DateTime day, decimal operationValue);

    Task UpdateAsync(string merchantId, DateTime day, decimal operationValue);
}
