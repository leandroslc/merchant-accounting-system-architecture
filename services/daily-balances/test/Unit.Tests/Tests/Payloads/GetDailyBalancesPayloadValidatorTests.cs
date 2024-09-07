using DailyBalances.Core.Payloads.GetDailyBalances;
using FluentValidation.TestHelper;

namespace DailyBalances.UnitTests.Tests.Payloads;

public class GetDailyBalancesPayloadValidatorTests
{
    private readonly GetDailyBalancesPayloadValidator validator = new();

    [Fact]
    public void Given_EmptyDay_Should_ReturnError()
    {
        var payload = new GetDailyBalancesPayload();

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Day)
            .WithErrorMessage("'day' must not be empty.");
    }

    [Fact]
    public void Given_InvalidDay_Should_ReturnError()
    {
        var payload = new GetDailyBalancesPayload
        {
            Day = "2024-02-31",
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Day)
            .WithErrorMessage("'day' must be a valid date.");
    }

    [Fact]
    public void Given_InvalidDayFormat_Should_ReturnError()
    {
        var payload = new GetDailyBalancesPayload
        {
            Day = "2024/05/10",
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldHaveValidationErrorFor(p => p.Day)
            .WithErrorMessage("'day' must be a date in the format YYYY-MM-DD.");
    }

    [Fact]
    public void Given_ProperPayload_Should_PassAllValidations()
    {
        var payload = new GetDailyBalancesPayload
        {
            Day = "2024-05-10",
        };

        // Act
        var result = validator.TestValidate(payload);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}
