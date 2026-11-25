using Fairmark.Application.Common.Exceptions;

namespace Fairmark.UnitTests.Common;

public class AppExceptionTests
{
    [Fact]
    public void ConflictException_KeepsCodeAndMessage()
    {
        var exception = new ConflictException("email_taken", "An account with this email already exists.");

        Assert.Equal("email_taken", exception.Code);
        Assert.Equal("An account with this email already exists.", exception.Message);
        Assert.IsAssignableFrom<AppException>(exception);
    }
}
