using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Seeders;

namespace Restaurants.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        return services;
    }


    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<RestaurantDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<ISeeder, RestaurantSeeder>();
        services.AddScoped<AppSeeder>();
        return services;
    }
}

