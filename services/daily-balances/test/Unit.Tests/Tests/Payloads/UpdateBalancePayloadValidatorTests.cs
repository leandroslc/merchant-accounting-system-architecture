using DailyBalances.Core.Entities.AccountingOperations;
using DailyBalances.Core.Payloads.UpdateBalance;
using FluentValidation.TestHelper;

namespace DailyBalances.UnitTests.Tests.Payloads;

public class UpdateBalancePayloadValidatorTests
{
    private readonly UpdateBalancePayloadValidator validator = new();

    [Fact]
    public void Given_EmptyMerchantId_Should_ReturnError()
    {
        var payload = new UpdateBalancePayload
        {
            RegistrationDate = DateTime.Now,
            Type = AccountingOperationType.Credit,
            Value = 100,
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.MerchantId)
            .WithErrorMessage("'MerchantId' must not be empty.");
    }

    [Fact]
    public void Given_EmptyRegistrationDate_Should_ReturnError()
    {
        var payload = new UpdateBalancePayload
        {
            MerchantId = "123",
            Type = AccountingOperationType.Credit,
            Value = 100,
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.RegistrationDate)
            .WithErrorMessage("'RegistrationDate' must not be empty.");
    }

    [Fact]
    public void Given_EmptyValue_Should_ReturnError()
    {
        var payload = new UpdateBalancePayload
        {
            MerchantId = "123",
            RegistrationDate = DateTime.Now,
            Type = AccountingOperationType.Credit,
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Value)
            .WithErrorMessage("'Value' must not be empty.");
    }

    [Fact]
    public void Given_ValueLessThanZero_Should_ReturnError()
    {
        var payload = new UpdateBalancePayload
        {
            MerchantId = "123",
            RegistrationDate = DateTime.Now,
            Type = AccountingOperationType.Credit,
            Value = -25,
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Value)
            .WithErrorMessage("'Value' must be greater than or equal to '0'.");
    }

    [Fact]
    public void Given_EmptyType_Should_ReturnError()
    {
        var payload = new UpdateBalancePayload
        {
            MerchantId = "123",
            RegistrationDate = DateTime.Now,
            Value = 100
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Type)
            .WithErrorMessage("'Type' must not be empty.");
    }

    [Fact]
    public void Given_InvalidType_Should_ReturnError()
    {
        var payload = new UpdateBalancePayload
        {
            MerchantId = "123",
            RegistrationDate = DateTime.Now,
            Type = 0,
            Value = 100
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Type)
            .WithErrorMessage("'Type' has a range of values which does not include '0'.");
    }

    [Fact]
    public void Given_ProperPayload_Should_PassAllValidations()
    {
        var payload = new UpdateBalancePayload
        {
            MerchantId = "123",
            RegistrationDate = DateTime.Now,
            Type = AccountingOperationType.Credit,
            Value = 100,
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
