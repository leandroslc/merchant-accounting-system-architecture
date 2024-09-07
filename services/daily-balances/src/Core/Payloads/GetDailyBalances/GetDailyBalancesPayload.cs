using System.Globalization;
using DailyBalances.Core.Queries.GetDailyBalances;

namespace DailyBalances.Core.Payloads.GetDailyBalances;

public sealed class GetDailyBalancesPayload
{
    public string? Day { get; set; }

    public GetDailyBalancesQuery AsGetDailyBalancesQuery(string merchantId)
    {
        return new GetDailyBalancesQuery
        {
            MerchantId = merchantId,
            Day = DateTime.SpecifyKind(
                DateTime.ParseExact(Day!, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                DateTimeKind.Utc),
        };
    }
}
