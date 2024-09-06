using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AccountingOperations.Core.Entities.Operations;
using AccountingOperations.Core.Events;
using AccountingOperations.IntegrationTests.Fixtures;
using FluentAssertions;
using FluentAssertions.Extensions;
using MassTransit.Internals;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;

namespace AccountingOperations.IntegrationTests;

[Collection(Collections.Api)]
public class OperationsTests
{
    private const string TestToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.mvdjdBZKOtWyQl54xAd8C9kY0RUyq-z26qNTjFR1DKA";
    private readonly HttpClient client;
    private readonly DbContext dbContext;
    private readonly ITestHarness messageExchangeTestHarness;

    public OperationsTests(ApiFixture api)
    {
        client = api.Client;
        dbContext = api.DbContext;
        messageExchangeTestHarness = api.MessageExchangeTestHarness;
    }

    [Theory]
    [InlineData("v1/operations/debit")]
    [InlineData("v1/operations/credit")]
    public async Task Given_InvalidAuthorizationToken_Should_ReturnUnauthorized(
        string uri)
    {
        var data = new
        {
            RegistrationDate = DateTime.Now,
            Value = 20.90,
        };

        // Act
        var response = await PostAsync(uri, data, "test");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("v1/operations/debit")]
    [InlineData("v1/operations/credit")]
    public async Task Given_InvalidData_Should_ReturnBadRequest(
        string uri)
    {
        var data = new
        {
            RegistrationDate = DateTime.Now,
            Value = "",
        };

        // Act
        var response = await PostAsync(uri, data, TestToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("v1/operations/debit", AccountingOperationType.Debit)]
    [InlineData("v1/operations/credit", AccountingOperationType.Credit)]
    public async Task Given_ValidData_Should_StoreOperation(
        string uri, AccountingOperationType expectedType)
    {
        var currentDate = DateTime.Now;

        var data = new
        {
            RegistrationDate = currentDate,
            Value = 20.90,
        };

        // Act
        await messageExchangeTestHarness.Start();

        await PostAsync(uri, data, TestToken);

        // Assert
        var expected = new
        {
            MerchantId = "1234567890",
            RegistrationDate = currentDate.AsUtc(),
            Type = expectedType,
            Value = 20.90,
        };

        var savedOperation = await dbContext.Set<AccountingOperation>().FirstOrDefaultAsync(a =>
            a.MerchantId == expected.MerchantId && a.RegistrationDate == expected.RegistrationDate);

        savedOperation
            .Should()
            .BeEquivalentTo(
                expected,
                config => config.Excluding(p => p.RegistrationDate));
    }

    [Theory]
    [InlineData("v1/operations/debit", AccountingOperationType.Debit)]
    public async Task Given_ValidData_Should_SendOperationRegisteredMessage(
        string uri, AccountingOperationType expectedType)
    {
        var currentDate = DateTime.Now;

        var data = new
        {
            RegistrationDate = currentDate,
            Value = 20.90,
        };

        // Act
        await messageExchangeTestHarness.Start();

        await PostAsync(uri, data, TestToken);

        // Assert
        var expected = new
        {
            MerchantId = "1234567890",
            RegistrationDate = currentDate.AsUtc(),
            Type = expectedType,
            Value = 20.90,
        };

        var sentMessage = await messageExchangeTestHarness.Sent
            .SelectAsync<OperationRegistered>()
            .FirstOrDefault();

        sentMessage.MessageObject
            .Should()
            .BeEquivalentTo(
                expected);

        await messageExchangeTestHarness.Stop();
    }

    private async Task<HttpResponseMessage> PostAsync<T>(string uri, T body, string token)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, uri);

        request.Content = JsonContent.Create(body);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await client.SendAsync(request);
    }
}
