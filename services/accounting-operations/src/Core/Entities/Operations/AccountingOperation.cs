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
        RegistrationDate = GetUTCDate(registrationDate);
        Value = value;
        Type = type;
    }

    public string MerchantId { get; }

    public DateTime RegistrationDate { get; }

    public decimal Value { get; }

    public AccountingOperationType Type { get; }

    private static DateTime GetUTCDate(DateTime dateTime)
    {
        return dateTime.Kind == DateTimeKind.Utc
            ? dateTime
            : DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
}
