using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Restaurants.API.Middlewares;

public class RequestLoggingFliter(ILogger<RequestLoggingFliter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var sw = Stopwatch.StartNew();

        var method = context.HttpContext.Request.Method;
        var path = context.HttpContext.Request.Path;
        var controller = context.ActionDescriptor.RouteValues["controller"];
        var action = context.ActionDescriptor.RouteValues["action"];

        await next();

        sw.Stop();

        var elapsedMs = sw.Elapsed.TotalMilliseconds;

        logger.LogInformation("{Method} {Path} - {Controller}/{Action} - {ElapsedMilliseconds}ms",
           method, path, controller, action, elapsedMs);

    }
}
