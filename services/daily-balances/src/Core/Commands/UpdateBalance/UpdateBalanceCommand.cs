using MediatR;

namespace DailyBalances.Core.Commands.UpdateBalance;

public sealed class UpdateBalanceCommand : IRequest
{
    public required string MerchantId { get; init; }

    public required DateTime Day { get; init; }

    public required decimal OperationValue { get; init; }
}
