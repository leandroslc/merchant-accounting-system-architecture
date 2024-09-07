using MediatR;

namespace DailyBalances.Core.Queries.GetDailyBalances;

public sealed class GetDailyBalancesQuery : IRequest<GetDailyBalancesQueryOutput>
{
    public required string MerchantId { get; init; }

    public required DateTime Day { get; init; }
}
