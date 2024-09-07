using DailyBalances.Core.Infrastructure.Repositories;
using MediatR;

namespace DailyBalances.Core.Commands.UpdateBalance;

public sealed class UpdateBalanceCommandHandler
    : IRequestHandler<UpdateBalanceCommand>
{
    private readonly IBalanceRepository balanceRepository;

    public UpdateBalanceCommandHandler(
        IBalanceRepository balanceRepository)
    {
        this.balanceRepository = balanceRepository;
    }

    public async Task Handle(
        UpdateBalanceCommand command,
        CancellationToken cancellationToken)
    {
        var balance = await balanceRepository.FindAsync(command.MerchantId, command.Day);

        if (balance is null)
        {
            await balanceRepository.CreateOrUpdateAsync(
                command.MerchantId,
                command.Day,
                command.OperationValue);

            return;
        }

        await balanceRepository.UpdateAsync(
            command.MerchantId,
            command.Day,
            command.OperationValue);
    }
}
