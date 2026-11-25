namespace Fairmark.Application.Common.Exceptions;

/// <summary>
/// Base class for expected failures of a use case (a business rule is broken, a resource is missing, the user
/// may not do this). The global exception handler in the API maps each subclass to an HTTP status code and
/// returns <see cref="Code"/> to the client (ADR-002, section 5).
/// </summary>
/// <param name="code">Stable machine-readable error code, for example <c>email_taken</c>. The frontend translates it.</param>
/// <param name="message">Description for logs and developers. It is not shown to the user.</param>
public abstract class AppException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
}
