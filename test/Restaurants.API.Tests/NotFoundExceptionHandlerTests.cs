using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Tests;
public class NotFoundExceptionHandlerTests
{
    private readonly Mock<IProblemDetailsService> _problemDetailsServiceMock;
    private readonly NotFoundExceptionHandler _handler;

    public NotFoundExceptionHandlerTests()
    {
        _problemDetailsServiceMock = new Mock<IProblemDetailsService>();
        _handler = new NotFoundExceptionHandler(_problemDetailsServiceMock.Object);
    }

    [Fact]
    public async Task TryHandleAsync_WithNotFoundException_ReturnsTrueAndWritesProblemDetails()
    {
        ProblemDetails? capturedProblemDetails = null;
        _problemDetailsServiceMock
            .Setup(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
            .Callback<ProblemDetailsContext>(ctx => capturedProblemDetails = ctx.ProblemDetails)
            .ReturnsAsync(true);

        var httpContext = new DefaultHttpContext();
        var exception = new NotFoundException("Type", "identifier");

        var result = await _handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        result.Should().BeTrue();
        capturedProblemDetails.Should().NotBeNull();
        capturedProblemDetails.Status.Should().Be(StatusCodes.Status404NotFound);
        _problemDetailsServiceMock.Verify(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);

    }

    [Fact]
    public async Task TryHandleAsync_WithNonNotFoundException_ReturnsFalse()
    {
        var httpContext = new DefaultHttpContext();
        var exception = new Exception("Other error");

        var result = await _handler.TryHandleAsync(httpContext, exception, CancellationToken.None);

        result.Should().BeFalse();
        _problemDetailsServiceMock.Verify(s => s.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Never);
    }
}