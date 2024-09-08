extern alias Api;

using DailyBalances.Api.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using ApiProgram = Api::Program;

namespace DailyBalances.IntegrationTests.Fixtures;

public class ApiFixture : BaseFixture<ApiProgram>
{
    public HttpClient Client { get; private set; } = null!;

    public override WebApplicationFactory<ApiProgram> CreateFactory()
    {
        return new WebApplicationFactory<ApiProgram>()
            .WithWebHostBuilder(builder => builder
                .UseEnvironment("Test")
                .ConfigureServices((context, services) => services
                    .ConfigureMigrations(context.Configuration)));
    }

    public override async Task DisposeAsync()
    {
        await base.DisposeAsync();

        Client.Dispose();
    }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        Client = Factory.CreateClient();
    }
}
