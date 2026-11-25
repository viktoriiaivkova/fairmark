namespace Fairmark.Application.Common.Exceptions;

/// <summary>The requested resource does not exist. Mapped to 404.</summary>
public sealed class NotFoundException(string code, string message) : AppException(code, message);
