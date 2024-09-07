using DailyBalances.Core.Entities.AccountingOperations;
using DailyBalances.Core.Payloads.UpdateBalance;
using FluentAssertions;
using FluentAssertions.Extensions;

namespace DailyBalances.UnitTests.Tests.Payloads;

public class UpdateBalancePayloadTests
{
    [Fact]
    public void AsUpdateBalanceCommand_Given_DebitType_Should_ReturnUpdateBalanceCommand()
    {
        var payload = new UpdateBalancePayload
        {
            MerchantId = "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617",
            RegistrationDate = new DateTime(2024, 05, 10),
            Type = AccountingOperationType.Debit,
            Value = 25.89M,
        };

        // Act
        var command = payload.AsUpdateBalanceCommand();

        // Assert
        command.Should().BeEquivalentTo(new
        {
            MerchantId = "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617",
            Day = new DateTime(2024, 05, 10).AsUtc(),
            OperationValue = 25.89,
        });
    }

    [Fact]
    public void AsUpdateBalanceCommand_Given_CreditType_Should_ReturnUpdateBalanceCommand()
    {
        var payload = new UpdateBalancePayload
        {
            MerchantId = "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617",
            RegistrationDate = new DateTime(2024, 05, 10),
            Type = AccountingOperationType.Credit,
            Value = 25.89M,
        };

        // Act
        var command = payload.AsUpdateBalanceCommand();

        // Assert
        command.Should().BeEquivalentTo(new
        {
            MerchantId = "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617",
            Day = new DateTime(2024, 05, 10).AsUtc(),
            OperationValue = -25.89,
        });
    }
}
