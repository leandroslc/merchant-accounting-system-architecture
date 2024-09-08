using AccountingOperations.Core.Events;
using DailyBalances.Core.Entities.AccountingOperations;
using DailyBalances.Core.Entities.Balances;
using DailyBalances.IntegrationTests.Fixtures;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;

namespace DailyBalances.IntegrationTests.Tests;

[Collection(Collections.OperationsConsumer)]
public class OperationsConsumerTests
{
    private readonly ITestHarness messageExchangeTestHarness;
    private readonly DbContext dbContext;

    public OperationsConsumerTests(OperationsConsumerFixture consumer)
    {
        dbContext = consumer.DbContext;
        messageExchangeTestHarness = consumer.MessageExchangeTestHarness;
    }

    [Fact]
    public async Task Given_BalanceExistsForTheDay_Should_SumValueToExistingBalance()
    {
        var merchantId = "321";
        var currentDate = DateTime.UtcNow.Date;

        var message = new OperationRegistered
        {
            MerchantId = merchantId,
            RegistrationDate = currentDate,
            Value = 120,
            Type = AccountingOperationType.Debit,
        };

        await RemoveBalanceIfExists(merchantId, currentDate);
        await AddBalance(merchantId, currentDate, 120);

        // Act
        await PublishMessageAndConsume(message);

        // Assert
        var addedBalance = await dbContext.Set<Balance>()
            .FindAsync(merchantId, currentDate);

        addedBalance.Should().BeEquivalentTo(new
        {
            MerchantId = merchantId,
            Day = currentDate,
            Total = 240.00M,
        });
    }

    [Fact]
    public async Task Given_NoBalanceExistsForTheDay_Should_AddValueToNewBalance()
    {
        var merchantId = "123";
        var currentDate = DateTime.UtcNow.Date;

        var message = new OperationRegistered
        {
            MerchantId = merchantId,
            RegistrationDate = currentDate,
            Value = 120,
            Type = AccountingOperationType.Debit,
        };

        await RemoveBalanceIfExists(merchantId, currentDate);

        // Act
        await PublishMessageAndConsume(message);

        // Assert
        var addedBalance = await dbContext.Set<Balance>()
            .FindAsync(merchantId, currentDate);

        addedBalance.Should().BeEquivalentTo(new
        {
            MerchantId = merchantId,
            Day = currentDate,
            Total = 120.00M,
        });
    }

    private async Task PublishMessageAndConsume(OperationRegistered message)
    {
        await messageExchangeTestHarness.Start();

        await messageExchangeTestHarness.Bus.Publish(message);

        await messageExchangeTestHarness.Consumed.Any<OperationRegistered>();

        // TODO: Find a better way to wait for consumer completion
        await Task.Delay(1000);

        await messageExchangeTestHarness.Stop();
    }

    private async Task AddBalance(string merchantId, DateTime day, decimal total)
    {
        dbContext.Set<Balance>().Add(new Balance(merchantId, day, total));

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();
    }

    private async Task RemoveBalanceIfExists(string merchantId, DateTime day)
    {
        var balance = await dbContext.Set<Balance>().FindAsync(merchantId, day);

        if (balance is not null)
        {
            dbContext.Set<Balance>().Remove(balance);

            await dbContext.SaveChangesAsync();

            dbContext.ChangeTracker.Clear();
        }
    }
}
