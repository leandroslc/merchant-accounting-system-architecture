using DailyBalances.Core.Commands.UpdateBalance;
using DailyBalances.Core.Entities.AccountingOperations;
using DailyBalances.Core.Services.OperationValue;

namespace DailyBalances.Core.Payloads.UpdateBalance;

public class UpdateBalancePayload
{
    public string? MerchantId { get; init; }

    public DateTime? RegistrationDate { get; init; }

    public decimal? Value { get; init; }

    public AccountingOperationType? Type { get; init; }

    public UpdateBalanceCommand AsUpdateBalanceCommand()
    {
        var operationValueService = OperationValueServiceFactory.GetByType(Type!.Value);

        return new UpdateBalanceCommand
        {
            MerchantId = MerchantId!,
            Day = DateTime.SpecifyKind(RegistrationDate!.Value.Date, DateTimeKind.Utc),
            OperationValue = operationValueService.GetValue(Value!.Value),
        };
    }
}
