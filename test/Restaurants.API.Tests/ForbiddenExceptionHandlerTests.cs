using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Tests;
public class ForbiddenExceptionHandlerTests
{
    private readonly Mock<IProblemDetailsService> _problemDetailsServiceMock;
    private readonly ForbiddenExceptionHandler _handler;

    public ForbiddenExceptionHandlerTests()
    {
        _problemDetailsServiceMock = new Mock<IProblemDetailsService>();
        _handler = new ForbiddenExceptionHandler(_problemDetailsServiceMock.Object);
    }


    [Fact]
    public async Task TryHandleAsync_WithForbiddenException_ReturnsTrueAndWritesProblemDetails()
    {
        ProblemDetails? capturedProblemDetails = null;

        _problemDetailsServiceMock
            .Setup(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
            .Callback<ProblemDetailsContext>(ctx => capturedProblemDetails = ctx.ProblemDetails)
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var exception = new ForbiddenException("Access is forbidden.");

        var result = await _handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        result.Should().BeTrue();
        capturedProblemDetails.Should().NotBeNull();
        capturedProblemDetails!.Title.Should().Be("Access Denied");
        capturedProblemDetails.Status.Should().Be(StatusCodes.Status403Forbidden);
        _problemDetailsServiceMock.Verify(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
    }

    [Fact]
    public async Task TryHandleAsync_WithNonForbiddenException_ReturnsFalseAndDoesNotWritesProblemDetails()
    {
        var httpContext = new DefaultHttpContext();
        var exception = new Exception("Some other exception.");

        var result = await _handler.TryHandleAsync(httpContext, exception, CancellationToken.None);
        result.Should().BeFalse();
        _problemDetailsServiceMock.Verify(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Never);
    }
}
