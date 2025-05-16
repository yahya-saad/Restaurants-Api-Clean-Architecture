using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Behaviors;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;

namespace Restaurants.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(CreateRestaurantCommandValidator).Assembly;

        services.AddAutoMapper(assembly);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddValidatorsFromAssembly(assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

}
