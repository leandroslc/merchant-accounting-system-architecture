using DailyBalances.Core.Entities.Balances;
using DailyBalances.Core.Infrastructure.Repositories;
using DailyBalances.Core.Queries.GetDailyBalances;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace DailyBalances.UnitTests.Tests.Queries;

public class GetDailyBalancesQueryHandlerTests
{
    private readonly IBalanceRepository balanceRepository;
    private readonly GetDailyBalancesQueryHandler handler;
    private readonly GetDailyBalancesQuery validQuery = new()
    {
        MerchantId = "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617",
        Day = new DateTime(2024, 05, 10),
    };

    public GetDailyBalancesQueryHandlerTests()
    {
        balanceRepository = Substitute.For<IBalanceRepository>();
        handler = new(balanceRepository);
    }

    [Fact]
    public async Task Should_SearchStoredBalance()
    {
        // Act
        await handler.Handle(validQuery, CancellationToken.None);

        // Assert
        await balanceRepository
            .Received()
            .FindAsync(
                Arg.Is<string>(p => p == "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617"),
                Arg.Is<DateTime>(p => p == new DateTime(2024, 05, 10)));
    }

    [Fact]
    public async Task Given_HasBalanceStored_Should_ReturnCurrentBalance()
    {
        balanceRepository
            .FindAsync(Arg.Any<string>(), Arg.Any<DateTime>())
            .Returns(new Balance(validQuery.MerchantId, validQuery.Day, 300.00M));

        // Act
        var output = await handler.Handle(validQuery, CancellationToken.None);

        // Assert
        output.Should().BeEquivalentTo(new
        {
            Total = 300.00,
        });
    }

    [Fact]
    public async Task Given_NoBalanceIsFound_Should_ReturnZeroBalance()
    {
        balanceRepository
            .FindAsync(Arg.Any<string>(), Arg.Any<DateTime>())
            .ReturnsNull();

        // Act
        var output = await handler.Handle(validQuery, CancellationToken.None);

        // Assert
        output.Should().BeEquivalentTo(new
        {
            Total = 0,
        });
    }
}
