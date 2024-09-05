using AccountingOperations.Core.Commands.OperationRegistration;
using AccountingOperations.Core.Entities.Operations;

namespace AccountingOperations.Core.Payloads.OperationRegistration;

public sealed class RegisterOperationPayload
{
    public DateTime RegistrationDate { get; set; }

    public decimal Value { get; set; }

    public RegisterOperationCommand AsDebitRegistrationCommand(string merchantId)
        => AsRegisterOperationCommand(merchantId, AccountingOperationType.Debit);

    public RegisterOperationCommand AsCreditRegistrationCommand(string merchantId)
        => AsRegisterOperationCommand(merchantId, AccountingOperationType.Credit);

    private RegisterOperationCommand AsRegisterOperationCommand(
        string merchantId,
        AccountingOperationType type)
    {
        return new RegisterOperationCommand
        {
            MerchantId = merchantId,
            RegistrationDate = RegistrationDate,
            Value = Value,
            Type = type,
        };
    }
}
