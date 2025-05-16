
using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestLoggingMiddleware(ILogger<RequestLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var sw = Stopwatch.StartNew();

        await next(context);

        sw.Stop();

        var method = context.Request.Method;
        var path = context.Request.Path;

        var ellapsedMilliseconds = sw.ElapsedMilliseconds;

        logger.LogInformation(
            "Request {Method} {Path}  in {ElapsedMilliseconds}ms",
            method,
            path,
            ellapsedMilliseconds);

    }
}
