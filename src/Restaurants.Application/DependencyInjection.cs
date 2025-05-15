using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddScoped<IRestaurantService, RestaurantService>();

        services.AddAutoMapper(assembly);

        services.AddValidatorsFromAssembly(assembly);
        return services;
    }
}
