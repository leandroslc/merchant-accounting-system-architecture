using DailyBalances.Core.Commands.UpdateBalance;
using DailyBalances.Core.Entities.Balances;
using DailyBalances.Core.Infrastructure.Repositories;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace DailyBalances.UnitTests.Tests.Commands;

public class UpdateBalanceCommandHandlerTests
{
    private readonly IBalanceRepository balanceRepository;
    private readonly UpdateBalanceCommandHandler handler;
    private readonly UpdateBalanceCommand validCommand = new()
    {
        MerchantId = "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617",
        Day = new DateTime(2024, 05, 10),
        OperationValue = 23.89M,
    };

    public UpdateBalanceCommandHandlerTests()
    {
        balanceRepository = Substitute.For<IBalanceRepository>();
        handler = new(balanceRepository);
    }

    [Fact]
    public async Task Should_GetStoredBalance()
    {
        // Act
        await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        await balanceRepository
            .Received()
            .FindAsync(
                Arg.Is<string>(p => p == "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617"),
                Arg.Is<DateTime>(p => p == new DateTime(2024, 05, 10)));
    }

    [Fact]
    public async Task Given_HasBalanceStored_Should_UpdateBalance()
    {
        balanceRepository
            .FindAsync(Arg.Any<string>(), Arg.Any<DateTime>())
            .Returns(new Balance(validCommand.MerchantId, validCommand.Day, 300.00M));

        // Act
        await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        await balanceRepository
            .Received()
            .UpdateAsync(
                Arg.Is<string>(p => p == "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617"),
                Arg.Is<DateTime>(p => p == new DateTime(2024, 05, 10)),
                Arg.Is<decimal>(p => p == 23.89M));
    }

    [Fact]
    public async Task Given_NoBalanceIsFound_Should_CreateOrUpdateBalance()
    {
        balanceRepository
            .FindAsync(Arg.Any<string>(), Arg.Any<DateTime>())
            .ReturnsNull();

        // Act
        await handler.Handle(validCommand, CancellationToken.None);

        // Assert
        await balanceRepository
            .Received()
            .CreateOrUpdateAsync(
                Arg.Is<string>(p => p == "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617"),
                Arg.Is<DateTime>(p => p == new DateTime(2024, 05, 10)),
                Arg.Is<decimal>(p => p == 23.89M));
    }
}
