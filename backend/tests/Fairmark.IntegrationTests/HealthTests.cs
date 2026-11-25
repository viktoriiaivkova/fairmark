using System.Net;
using Fairmark.IntegrationTests.Infrastructure;

namespace Fairmark.IntegrationTests;

public class HealthTests(FairmarkApiFactory factory)
{
    [Fact]
    public async Task Health_ReturnsOk()
    {
        using var client = factory.CreateApiClient();

        var response = await client.GetAsync("/health", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
