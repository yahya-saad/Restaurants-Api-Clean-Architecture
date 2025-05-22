using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Domain.Exceptions;

namespace Restaurants.API.Middlewares;

public sealed class ForbiddenExceptionHandler(IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ForbiddenException forbiddenException)
        {
            return false;
        }
        var problemDetails = new ProblemDetails
        {
            Title = "Access Denied",
            Detail = "You do not have permission to access this resource.",
            Status = StatusCodes.Status403Forbidden
        };

        httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        var context = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails,
        };
        return await problemDetailsService.TryWriteAsync(context);
    }
}
