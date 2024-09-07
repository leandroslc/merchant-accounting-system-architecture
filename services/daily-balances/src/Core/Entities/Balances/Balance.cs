namespace DailyBalances.Core.Entities.Balances;

public sealed class Balance
{
    public Balance(
        string merchantId,
        DateTime day,
        decimal total)
    {
        MerchantId = merchantId;
        Day = day;
        Total = total;
    }

    public string MerchantId { get; }

    public DateTime Day { get; }

    public decimal Total { get; }
}
