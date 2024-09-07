using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DailyBalances.Core.Entities.Balances;
using DailyBalances.Core.Queries.GetDailyBalances;
using DailyBalances.IntegrationTests.Fixtures;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DailyBalances.IntegrationTests.Tests;

[Collection(Collections.Api)]
public class BalancesTests
{
    private const string TestToken
        = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.mvdjdBZKOtWyQl54xAd8C9kY0RUyq-z26qNTjFR1DKA";
    private const string Endpoint = "v1/balances";

    private readonly HttpClient client;
    private readonly DbContext dbContext;

    public BalancesTests(ApiFixture api)
    {
        client = api.Client;
        dbContext = api.DbContext;
    }

    [Fact]
    public async Task Given_InvalidAuthorizationToken_Should_ReturnUnauthorized()
    {
        // Act
        var response = await GetAsync(Endpoint, "day=2024-05-10", "test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Given_InvalidData_Should_ReturnBadRequest()
    {
        // Act
        var response = await GetAsync(Endpoint, "day=123", TestToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Given_ValidData_Should_ReturnSpecifiedDayBalance()
    {
        var merchantId = "1234567890";
        var currentDate = DateTime.UtcNow.Date;

        await RemoveBalanceIfExists(merchantId, currentDate);
        await AddBalance(merchantId, currentDate, 1500.00M);

        // Act
        var response = await GetAsync(Endpoint, $"day={currentDate:yyyy-MM-dd}", TestToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content
            .ReadFromJsonAsync<GetDailyBalancesQueryOutput>();

        responseContent!.Should().BeEquivalentTo(new
        {
            Total = 1500.00,
        });
    }

    [Fact]
    public async Task Given_ValidData_And_NoBalanceIsFound_Should_ReturnEmptyBalance()
    {
        var merchantId = "1234567890";
        var currentDate = DateTime.UtcNow.Date;

        await RemoveBalanceIfExists(merchantId, currentDate);

        // Act
        var response = await GetAsync(Endpoint, $"day={currentDate:yyyy-MM-dd}", TestToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseContent = await response.Content
            .ReadFromJsonAsync<GetDailyBalancesQueryOutput>();

        responseContent.Should().BeEquivalentTo(new
        {
            Total = 0,
        });
    }

    private async Task<HttpResponseMessage> GetAsync(string uri, string query, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{uri}?{query}");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await client.SendAsync(request);
    }

    private async Task AddBalance(string merchantId, DateTime day, decimal total)
    {
        dbContext.Set<Balance>().Add(new Balance(merchantId, day, total));

        await dbContext.SaveChangesAsync();
    }

    private async Task RemoveBalanceIfExists(string merchantId, DateTime day)
    {
        var balance = await dbContext.Set<Balance>().FindAsync(merchantId, day);

        if (balance is not null)
        {
            dbContext.Set<Balance>().Remove(balance);

            await dbContext.SaveChangesAsync();
        }
    }
}
