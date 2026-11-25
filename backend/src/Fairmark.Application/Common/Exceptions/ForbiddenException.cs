namespace Fairmark.Application.Common.Exceptions;

/// <summary>The user is authenticated but may not perform this action (for example, not a member of the team). Mapped to 403.</summary>
public sealed class ForbiddenException(string code, string message) : AppException(code, message);
