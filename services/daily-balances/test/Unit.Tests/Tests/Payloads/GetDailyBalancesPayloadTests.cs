using DailyBalances.Core.Payloads.GetDailyBalances;
using FluentAssertions;

namespace DailyBalances.UnitTests.Tests.Payloads;

public class GetDailyBalancesPayloadTests
{
    [Fact]
    public void AsGetDailyBalancesQuery_Should_ReturnGetDailyBalancesQuery()
    {
        var payload = new GetDailyBalancesPayload
        {
            Day = "2024-05-10",
        };

        // Act
        var command = payload.AsGetDailyBalancesQuery("8a61cbd3-d8b1-4ca7-b9a7-acb46b478617");

        // Assert
        command.Should().BeEquivalentTo(new
        {
            MerchantId = "8a61cbd3-d8b1-4ca7-b9a7-acb46b478617",
            Day = new DateTime(2024, 05, 10),
        });
    }
}
