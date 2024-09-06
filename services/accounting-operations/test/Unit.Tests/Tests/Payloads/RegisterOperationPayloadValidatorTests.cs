using AccountingOperations.Core.Payloads.OperationRegistration;
using FluentValidation.TestHelper;

namespace AccountingOperations.UnitTests.Tests.Payloads;

public class RegisterOperationPayloadValidatorTests
{
    private readonly RegisterOperationPayloadValidator validator = new();

    [Fact]
    public void Given_EmptyRegistrationDate_Should_ReturnError()
    {
        var payload = new RegisterOperationPayload
        {
            Value = 34.89M,
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.RegistrationDate)
            .WithErrorMessage("'registrationDate' must not be empty.");
    }

    [Fact]
    public void Given_NonUTCRegistrationDate_Should_ReturnError()
    {
        var payload = new RegisterOperationPayload
        {
            Value = 34.89M,
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.RegistrationDate)
            .WithErrorMessage("'registrationDate' must be an UTC date.");
    }

    [Fact]
    public void Given_EmptyValue_Should_ReturnError()
    {
        var payload = new RegisterOperationPayload
        {
            RegistrationDate = new DateTime(2024, 05, 10),
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Value)
            .WithErrorMessage("'value' must not be empty.");
    }

    [Fact]
    public void Given_ProperPayload_Should_PassAllValidations()
    {
        var payload = new RegisterOperationPayload
        {
            RegistrationDate = new DateTime(2024, 05, 10, 0, 0, 0, kind: DateTimeKind.Utc),
            Value = 23.99M,
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
