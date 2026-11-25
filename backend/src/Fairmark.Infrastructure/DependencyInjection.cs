using Fairmark.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Fairmark.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Name of the connection string in configuration: <c>ConnectionStrings:Default</c>.</summary>
    public const string ConnectionStringName = "Default";

    /// <summary>Registers the services that the Infrastructure project provides to the API.</summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // The options lambda runs the first time an AppDbContext is created, not at startup.
        // So the API starts without a database (useful for /health), and tests can replace
        // the connection string before the first request.
        services.AddDbContext<AppDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString(ConnectionStringName)
                ?? throw new InvalidOperationException(
                    $"Connection string 'ConnectionStrings:{ConnectionStringName}' is not configured.");

            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention(); // ADR-002, section 3
        });

        return services;
    }
}
