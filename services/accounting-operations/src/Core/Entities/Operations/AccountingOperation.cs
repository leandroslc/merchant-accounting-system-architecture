namespace AccountingOperations.Core.Entities.Operations;

public sealed class AccountingOperation
{
    public AccountingOperation(
        string merchantId,
        DateTime registrationDate,
        decimal value,
        AccountingOperationType type)
    {
        MerchantId = merchantId;
        RegistrationDate = registrationDate;
        Value = value;
        Type = type;
    }

    public string MerchantId { get; }

    public DateTime RegistrationDate { get; }

    public decimal Value { get; }

    public AccountingOperationType Type { get; }
}
