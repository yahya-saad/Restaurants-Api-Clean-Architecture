using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Middlewares;

namespace Restaurants.API.Tests;
public class ValidationExceptionHandlerTests
{
    private readonly Mock<IProblemDetailsService> _problemDetailsServiceMock;
    private readonly ValidationExceptionHandler _handler;

    public ValidationExceptionHandlerTests()
    {
        _problemDetailsServiceMock = new Mock<IProblemDetailsService>();
        _handler = new ValidationExceptionHandler(_problemDetailsServiceMock.Object);
    }

    [Fact]
    public async Task TryHandleAsync_WithValidationException_ReturnsTrueAndWritesProblemDetails()
    {
        ProblemDetails? capturedProblemDetails = null;
        _problemDetailsServiceMock
            .Setup(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
            .Callback<ProblemDetailsContext>(ctx => capturedProblemDetails = ctx.ProblemDetails)
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var exception = new ValidationException("Validation failed.");

        var result = await _handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        result.Should().BeTrue();
        capturedProblemDetails.Should().NotBeNull();
        capturedProblemDetails.Status.Should().Be(StatusCodes.Status400BadRequest);
        _problemDetailsServiceMock.Verify(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
    }

    [Fact]
    public async Task TryHandleAsync_WithNonValidationException_ReturnsFalse()
    {
        var httpContext = new DefaultHttpContext();
        var exception = new Exception("Other error");

        var result = await _handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        result.Should().BeFalse();
        _problemDetailsServiceMock.Verify(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Never);
    }
}