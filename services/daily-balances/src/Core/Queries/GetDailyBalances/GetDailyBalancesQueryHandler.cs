using DailyBalances.Core.Infrastructure.Repositories;
using MediatR;

namespace DailyBalances.Core.Queries.GetDailyBalances;

public sealed class GetDailyBalancesQueryHandler
    : IRequestHandler<GetDailyBalancesQuery, GetDailyBalancesQueryOutput>
{
    private readonly IBalanceRepository balanceRepository;

    public GetDailyBalancesQueryHandler(
        IBalanceRepository balanceRepository)
    {
        this.balanceRepository = balanceRepository;
    }

    public async Task<GetDailyBalancesQueryOutput> Handle(
        GetDailyBalancesQuery query,
        CancellationToken cancellationToken)
    {
        var balance = await balanceRepository.FindAsync(
            query.MerchantId, query.Day);

        var total = balance is not null
            ? balance.Total
            : decimal.Zero;

        return new GetDailyBalancesQueryOutput(total);
    }
}
