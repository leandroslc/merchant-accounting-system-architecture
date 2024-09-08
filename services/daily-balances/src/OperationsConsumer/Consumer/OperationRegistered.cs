using DailyBalances.Core.Entities.AccountingOperations;
using DailyBalances.Core.Payloads.UpdateBalance;

namespace AccountingOperations.Core.Events;

public sealed class OperationRegistered
{
    public string? MerchantId { get; init; }

    public DateTime? RegistrationDate { get; init; }

    public decimal? Value { get; init; }

    public AccountingOperationType? Type { get; init; }

    public UpdateBalancePayload ToUpdateBalancePayload()
    {
        return new UpdateBalancePayload
        {
            MerchantId = MerchantId,
            RegistrationDate = RegistrationDate,
            Value = Value,
            Type = Type,
        };
    }
}
