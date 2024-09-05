using AccountingOperations.Core.Entities.Operations;
using MediatR;

namespace AccountingOperations.Core.Commands.OperationRegistration;

public sealed class RegisterOperationCommand : IRequest
{
    public required string MerchantId { get; init; }

    public required DateTime RegistrationDate { get; init; }

    public required decimal Value { get; init; }

    public required AccountingOperationType Type { get; init; }

    public AccountingOperation ToAccountingOperation()
    {
        return new AccountingOperation(
            merchantId: MerchantId,
            registrationDate: RegistrationDate,
            value: Value,
            type: Type);
    }
}
