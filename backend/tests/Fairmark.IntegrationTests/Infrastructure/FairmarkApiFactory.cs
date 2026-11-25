using Fairmark.Infrastructure;
using Fairmark.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

// One API instance and one database container for the whole test run (xUnit v3 assembly fixture).
[assembly: AssemblyFixture(typeof(Fairmark.IntegrationTests.Infrastructure.FairmarkApiFactory))]

namespace Fairmark.IntegrationTests.Infrastructure;

/// <summary>
/// Starts the API in memory against a real PostgreSQL running in Docker (Testcontainers).
/// Docker must be running on the machine.
/// </summary>
public sealed class FairmarkApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    // The same major version as the local database in Docker Compose (FM-45).
    private readonly PostgreSqlContainer _database = new PostgreSqlBuilder("postgres:18-alpine").Build();

    public async ValueTask InitializeAsync()
    {
        await _database.StartAsync();

        // A fresh database gets all migrations before the tests run (ADR-002, section 4).
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    /// <summary>An HTTP client for the in-memory API that asks for JSON, like the web application does.</summary>
    public HttpClient CreateApiClient()
    {
        var client = CreateClient();
        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
        return client;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Not Development: errors must look the way they do in production.
        builder.UseEnvironment("Testing");
        builder.UseSetting(
            $"ConnectionStrings:{DependencyInjection.ConnectionStringName}", _database.GetConnectionString());

        // Controllers that exist only in the test project (see TestErrorsController).
        builder.ConfigureTestServices(services =>
            services.AddControllers().AddApplicationPart(typeof(FairmarkApiFactory).Assembly));
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await _database.DisposeAsync();
    }
}
