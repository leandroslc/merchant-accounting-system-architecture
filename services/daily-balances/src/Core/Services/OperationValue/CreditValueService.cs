namespace DailyBalances.Core.Services.OperationValue;

public sealed class CreditValueService : IOperationValueService
{
    public decimal GetValue(decimal value)
    {
        return value * -1;
    }
}
