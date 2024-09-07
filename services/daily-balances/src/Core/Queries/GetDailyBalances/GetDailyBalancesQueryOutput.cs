namespace DailyBalances.Core.Queries.GetDailyBalances;

public sealed class GetDailyBalancesQueryOutput
{
    public GetDailyBalancesQueryOutput(decimal total)
    {
        Total = total;
    }

    public decimal Total { get; }
}
