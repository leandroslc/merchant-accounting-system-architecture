using AccountingOperations.Core.Commands.OperationRegistration;
using AccountingOperations.Core.Entities.Operations;
using FluentAssertions;

namespace AccountingOperations.UnitTests.Tests.Commands;

public class RegisterOperationCommandTests
{
    [Fact]
    public void ToAccountingOperation_Should_CreateAccountingOperation()
    {
        var command = new RegisterOperationCommand
        {
            MerchantId = "3a16f3cb-717e-4135-9513-9059af87b675",
            RegistrationDate = new DateTime(2024, 05, 10),
            Value = 21.12M,
            Type = AccountingOperationType.Credit,
        };

        // Act
        var operation = command.ToAccountingOperation();

        // Assert
        operation.Should().BeEquivalentTo(new
        {
            MerchantId = "3a16f3cb-717e-4135-9513-9059af87b675",
            RegistrationDate = new DateTime(2024, 05, 10),
            Value = 21.12M,
            Type = AccountingOperationType.Credit,
        });
    }
}
