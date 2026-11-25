using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fairmark.Api.Health;

/// <summary>
/// Writes the /health response as JSON: the overall status and the status of each check.
/// Only statuses are returned; error details stay in the log.
/// </summary>
public static class HealthCheckResponseWriter
{
    public static Task WriteAsync(HttpContext context, HealthReport report)
    {
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.ToDictionary(entry => entry.Key, entry => entry.Value.Status.ToString()),
        };

        return context.Response.WriteAsJsonAsync(response, context.RequestAborted);
    }
}
