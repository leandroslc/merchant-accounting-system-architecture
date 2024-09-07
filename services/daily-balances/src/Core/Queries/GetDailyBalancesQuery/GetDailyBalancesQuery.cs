using MediatR;

namespace DailyBalances.Core.Queries.GetDailyBalancesQuery;

public sealed class GetDailyBalancesQuery : IRequest<GetDailyBalancesQueryOutput>
{
    public required string MerchantId { get; init; }

    public required DateTime Day { get; init; }
}
