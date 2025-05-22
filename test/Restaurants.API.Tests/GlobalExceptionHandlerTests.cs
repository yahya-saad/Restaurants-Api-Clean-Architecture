using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Restaurants.API.Middlewares;

namespace Restaurants.API.Tests;
public class GlobalExceptionHandlerTests
{
    private readonly Mock<IProblemDetailsService> _problemDetailsServiceMock;
    private readonly Mock<ILogger<GlobalExceptionHandler>> _loggerMock;
    private readonly GlobalExceptionHandler _handler;

    public GlobalExceptionHandlerTests()
    {
        _problemDetailsServiceMock = new Mock<IProblemDetailsService>();
        _loggerMock = new Mock<ILogger<GlobalExceptionHandler>>();
        _handler = new GlobalExceptionHandler(_problemDetailsServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task TryHandleAsync_WithAnyException_ReturnsTrueAndWritesProblemDetails()
    {
        ProblemDetails? capturedProblemDetails = null;
        _problemDetailsServiceMock
            .Setup(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
            .Callback<ProblemDetailsContext>(ctx => capturedProblemDetails = ctx.ProblemDetails)
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var exception = new Exception("Unhandled error");

        var result = await _handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        result.Should().BeTrue();
        capturedProblemDetails.Should().NotBeNull();
        capturedProblemDetails!.Title.Should().Be("Internal Server Error");
        capturedProblemDetails.Detail.Should().Be("An error occurred while processing your request.");
        httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        _problemDetailsServiceMock.Verify(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
    }
}
