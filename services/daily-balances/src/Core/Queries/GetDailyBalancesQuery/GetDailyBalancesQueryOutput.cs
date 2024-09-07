using DailyBalances.Core.Entities.Balances;

namespace DailyBalances.Core.Queries.GetDailyBalancesQuery;

public sealed class GetDailyBalancesQueryOutput
{
    public GetDailyBalancesQueryOutput(Balance balance)
        : this(balance.Day)
    {
        Total = balance.Total;
    }

    public GetDailyBalancesQueryOutput(DateTime day)
    {
        Day = day;
        Total = 0;
    }

    public DateTime Day { get; }

    public decimal Total { get; }
}
