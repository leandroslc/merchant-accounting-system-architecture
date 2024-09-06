using AccountingOperations.Core.Entities.Operations;
using AccountingOperations.Core.Payloads.OperationRegistration;
using FluentAssertions;

namespace AccountingOperations.UnitTests.Tests.Payloads;

public class RegisterOperationPayloadTests
{
    [Fact]
    public void AsDebitRegistrationCommand_Should_ReturnDebitRegistrationCommand()
    {
        var payload = new RegisterOperationPayload
        {
            RegistrationDate = new DateTime(2024, 05, 10),
            Value = 34.89M,
        };

        // Act
        var command = payload.AsDebitRegistrationCommand("8a61cbd3-d8b1-4ca7-b9a7-acb46b478617");

        // Assert
        command.Should().BeEquivalentTo(new
        {
            MerchantId = "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617",
            RegistrationDate = new DateTime(2024, 05, 10),
            Value = 34.89,
            Type = AccountingOperationType.Debit,
        });
    }

    [Fact]
    public void AsCreditRegistrationCommand_Should_ReturnDebitRegistrationCommand()
    {
        var payload = new RegisterOperationPayload
        {
            RegistrationDate = new DateTime(2024, 06, 11),
            Value = 23.9M,
        };

        // Act
        var command = payload.AsCreditRegistrationCommand("a2ac63ad-e6ca-4abb-a432-212422c96680");

        // Assert
        command.Should().BeEquivalentTo(new
        {
            MerchantId = "a2ac63ad-e6ca-4abb-a432-212422c96680",
            RegistrationDate = new DateTime(2024, 06, 11),
            Value = 23.90,
            Type = AccountingOperationType.Credit,
        });
    }
}
