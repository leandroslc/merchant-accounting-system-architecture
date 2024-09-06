using AccountingOperations.Core.Entities.Operations;

namespace AccountingOperations.Core.Events;

public sealed class OperationRegistered
{
    public OperationRegistered(AccountingOperation operation)
    {
        MerchantId = operation.MerchantId;
        RegistrationDate = operation.RegistrationDate;
        Value = operation.Value;
        Type = operation.Type;
    }

    public string MerchantId { get; }

    public DateTime RegistrationDate { get; }

    public decimal Value { get; }

    public AccountingOperationType Type { get; }
}
