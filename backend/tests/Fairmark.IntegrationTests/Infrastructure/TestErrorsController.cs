using Fairmark.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Fairmark.IntegrationTests.Infrastructure;

/// <summary>
/// Test-only endpoints that throw each kind of exception, to check how the API reports errors.
/// Registered only in the integration tests; it is not part of the application.
/// </summary>
[ApiController]
[Route("api/test-errors")]
public sealed class TestErrorsController : ControllerBase
{
    [HttpGet("not-found")]
    public IActionResult ThrowNotFound() =>
        throw new NotFoundException("test_not_found", "Not found, raised by a test.");

    [HttpGet("forbidden")]
    public IActionResult ThrowForbidden() =>
        throw new ForbiddenException("test_forbidden", "Forbidden, raised by a test.");

    [HttpGet("conflict")]
    public IActionResult ThrowConflict() =>
        throw new ConflictException("test_conflict", "Conflict, raised by a test.");

    [HttpGet("unexpected")]
    public IActionResult ThrowUnexpected() =>
        throw new InvalidOperationException("Unexpected failure, raised by a test.");
}
