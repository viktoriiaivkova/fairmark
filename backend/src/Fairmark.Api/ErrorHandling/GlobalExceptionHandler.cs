using Fairmark.Application.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Fairmark.Api.ErrorHandling;

/// <summary>
/// Turns exceptions into ProblemDetails responses (ADR-002, section 5).
/// Exceptions from the Application layer become 403 / 404 / 409 with their machine-readable code;
/// anything else is an unexpected error: 500 with the code <c>internal_error</c>, details only in the log.
/// </summary>
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public const string InternalErrorCode = "internal_error";

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, code) = exception switch
        {
            NotFoundException e => (StatusCodes.Status404NotFound, e.Code),
            ForbiddenException e => (StatusCodes.Status403Forbidden, e.Code),
            ConflictException e => (StatusCodes.Status409Conflict, e.Code),
            _ => (StatusCodes.Status500InternalServerError, InternalErrorCode),
        };

        if (status == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(exception, "Unhandled exception");
        }
        else
        {
            logger.LogInformation("Request rejected with {Code}: {Message}", code, exception.Message);
        }

        httpContext.Response.StatusCode = status;

        // Title, type and traceId are filled in by the ProblemDetails service from the status code.
        var problem = new ProblemDetails { Status = status };
        problem.Extensions["code"] = code;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception,
        });
    }
}
