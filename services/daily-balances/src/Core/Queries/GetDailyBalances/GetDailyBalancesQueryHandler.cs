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
        GetDailyBalancesQuery request,
        CancellationToken cancellationToken)
    {
        var balance = await balanceRepository.Find(
            request.MerchantId, request.Day);

        if (balance is null)
        {
            return new GetDailyBalancesQueryOutput(request.Day);
        }

        return new GetDailyBalancesQueryOutput(balance);
    }
}
