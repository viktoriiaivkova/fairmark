namespace Fairmark.Application.Common.Exceptions;

/// <summary>A business rule or the current state does not allow the action (for example, a duplicate email). Mapped to 409.</summary>
public sealed class ConflictException(string code, string message) : AppException(code, message);
