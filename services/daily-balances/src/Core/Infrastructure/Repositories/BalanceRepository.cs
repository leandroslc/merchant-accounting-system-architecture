using DailyBalances.Core.Entities.Balances;
using DailyBalances.Core.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DailyBalances.Core.Infrastructure.Repositories;

public sealed class BalanceRepository : IBalanceRepository
{
    private readonly BalancesDbContext context;

    public BalanceRepository(BalancesDbContext context)
    {
        this.context = context;
    }

    public async Task<Balance?> FindAsync(string merchantId, DateTime balanceDay)
    {
        return await context.Set<Balance>().FindAsync(merchantId, balanceDay);
    }

    public async Task UpdateAsync(string merchantId, DateTime day, decimal operationValue)
    {
        await context
            .Set<Balance>()
            .Where(b => b.MerchantId == merchantId && b.Day == day)
            .ExecuteUpdateAsync(u =>
                u.SetProperty(p => p.Total, p => p.Total + operationValue));
    }

    public async Task CreateOrUpdateAsync(string merchantId, DateTime day, decimal operationValue)
    {
        try
        {
            var balance = new Balance(merchantId, day, operationValue);

            context.Add(balance);

            await context.SaveChangesAsync();
        }
        catch (DbUpdateException exception)
            when (VioletesUniqueConstraint(exception))
        {
            await UpdateAsync(merchantId, day, operationValue);
        }
    }

    private static bool VioletesUniqueConstraint(DbUpdateException exception)
    {
        var innerException = exception.InnerException;

        while (innerException is not null)
        {
            if (innerException.Message.Contains("pk_balances"))
            {
                return true;
            }

            innerException = innerException.InnerException;
        }

        return false;
    }
}
