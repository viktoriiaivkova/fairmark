using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Fairmark.Infrastructure;
using Fairmark.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Hosting;

namespace Fairmark.IntegrationTests;

public class HealthTests(FairmarkApiFactory factory)
{
    [Fact]
    public async Task Health_WhenDatabaseIsReachable_ReportsHealthy()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        using var client = factory.CreateApiClient();

        var response = await client.GetAsync("/health", cancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var health = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        Assert.Equal("Healthy", health.GetProperty("status").GetString());
        Assert.Equal("Healthy", health.GetProperty("checks").GetProperty("database").GetString());
    }

    [Fact]
    public async Task Health_WhenDatabaseIsUnreachable_Returns503AndReportsDatabaseUnhealthy()
    {
        var cancellationToken = TestContext.Current.CancellationToken;

        // The same API, but pointed at a port where no database listens.
        await using var factoryWithoutDatabase = factory.WithWebHostBuilder(builder =>
            builder.UseSetting(
                $"ConnectionStrings:{DependencyInjection.ConnectionStringName}",
                "Host=127.0.0.1;Port=1;Database=none;Username=none;Password=none;Timeout=2"));
        using var client = factoryWithoutDatabase.CreateClient();

        var response = await client.GetAsync("/health", cancellationToken);

        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        var health = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        Assert.Equal("Unhealthy", health.GetProperty("status").GetString());
        Assert.Equal("Unhealthy", health.GetProperty("checks").GetProperty("database").GetString());
    }
}
