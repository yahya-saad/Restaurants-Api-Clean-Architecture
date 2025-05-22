using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares;

public sealed class NotFoundExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }
        var problemDetails = new ProblemDetails
        {
            Title = "Resource Not Found",
            Detail = notFoundException.Message,
            Status = StatusCodes.Status404NotFound
        };

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails,
        };
        return await problemDetailsService.TryWriteAsync(context);
    }
}
