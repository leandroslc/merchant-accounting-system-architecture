using DailyBalances.Core.Commands.UpdateBalance;
using DailyBalances.Core.Entities.AccountingOperations;
using DailyBalances.Core.Services.OperationValue;

namespace DailyBalances.Core.Payloads.UpdateBalance;

public sealed class UpdateBalancePayload
{
    public required string MerchantId { get; init; }

    public required DateTime RegistrationDate { get; init; }

    public required decimal Value { get; init; }

    public required AccountingOperationType Type { get; init; }

    public UpdateBalanceCommand AsUpdateBalanceCommand()
    {
        var operationValueService = OperationValueServiceFactory.GetByType(Type);

        return new UpdateBalanceCommand
        {
            MerchantId = MerchantId,
            Day = DateTime.SpecifyKind(RegistrationDate, DateTimeKind.Utc),
            OperationValue = operationValueService.GetValue(Value),
        };
    }
}
