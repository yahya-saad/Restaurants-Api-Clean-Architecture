using Restaurants.API.Filters;
using Restaurants.API.Middlewares;
using Restaurants.Domain.Entities;
using Serilog;

namespace Restaurants.API.Extensions;

public static class WebApplicatiobBuilderExtensions
{
    public static void AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication();
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<RequestLoggingFliter>();
        }).AddNewtonsoftJson();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwagger();

        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });


        builder.Services.AddIdentityApiEndpoints<User>();


        #region exception handlers
        builder.Services.AddExceptionHandler<ForbiddenExceptionHandler>();
        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<NotFoundExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        #endregion

    }
}
