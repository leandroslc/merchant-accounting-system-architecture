using DailyBalances.Core.Entities.AccountingOperations;

namespace DailyBalances.Core.Services.OperationValue;

public static class OperationValueServiceFactory
{
    private static readonly Dictionary<AccountingOperationType, IOperationValueService> services
        = new()
        {
            [AccountingOperationType.Credit] = new CreditValueService(),
            [AccountingOperationType.Debit] = new DebitValueService(),
        };

    public static IOperationValueService GetByType(AccountingOperationType type)
    {
        if (services.TryGetValue(type, out var service))
        {
            return service;
        }

        throw new InvalidOperationException($"No service found matching {type}");
    }
}
