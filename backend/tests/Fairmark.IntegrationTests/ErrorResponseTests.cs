using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Fairmark.IntegrationTests.Infrastructure;

namespace Fairmark.IntegrationTests;

/// <summary>Every error is returned as ProblemDetails (ADR-002, section 5).</summary>
public class ErrorResponseTests(FairmarkApiFactory factory)
{
    private const string ProblemJson = "application/problem+json";

    [Fact]
    public async Task UnknownUrl_Returns404AsProblemDetails()
    {
        using var client = factory.CreateApiClient();

        var response = await client.GetAsync("/api/does-not-exist", TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(ProblemJson, response.Content.Headers.ContentType?.MediaType);
    }

    [Theory]
    [InlineData("not-found", HttpStatusCode.NotFound, "test_not_found")]
    [InlineData("forbidden", HttpStatusCode.Forbidden, "test_forbidden")]
    [InlineData("conflict", HttpStatusCode.Conflict, "test_conflict")]
    [InlineData("unexpected", HttpStatusCode.InternalServerError, "internal_error")]
    public async Task Exception_IsReturnedAsProblemDetailsWithCode(
        string endpoint, HttpStatusCode expectedStatus, string expectedCode)
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        using var client = factory.CreateApiClient();

        var response = await client.GetAsync($"/api/test-errors/{endpoint}", cancellationToken);

        Assert.Equal(expectedStatus, response.StatusCode);
        Assert.Equal(ProblemJson, response.Content.Headers.ContentType?.MediaType);

        var problem = await response.Content.ReadFromJsonAsync<JsonElement>(cancellationToken);
        Assert.Equal(expectedCode, problem.GetProperty("code").GetString());
        Assert.Equal((int)expectedStatus, problem.GetProperty("status").GetInt32());
    }

    [Fact]
    public async Task UnexpectedError_DoesNotExposeExceptionDetails()
    {
        var cancellationToken = TestContext.Current.CancellationToken;
        using var client = factory.CreateApiClient();

        var response = await client.GetAsync("/api/test-errors/unexpected", cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        Assert.DoesNotContain("Unexpected failure, raised by a test.", body);
        Assert.DoesNotContain("InvalidOperationException", body);
    }
}
