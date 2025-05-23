using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Authorization;
using Restaurants.Infrastructure.Authorization.Requirement;
using Restaurants.Infrastructure.Authorization.Services;
using Restaurants.Infrastructure.Configurations;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Repositories;
using Restaurants.Infrastructure.Seeders;
using Restaurants.Infrastructure.Storage;

namespace Restaurants.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration)
            .AddIdentity()
            .AddAuthorization()
            .AddBlobStorage();


        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<RestaurantDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<ISeeder, RestaurantSeeder>();
        services.AddScoped<ISeeder, DishSeeder>();
        services.AddScoped<ISeeder, RolesSeeder>();
        services.AddScoped<AppSeeder>();


        services.AddScoped<IRestaurantsRepository, RestaurantRepository>();
        services.AddScoped<IDishesRepository, DishesRepository>();
        return services;
    }

    private static IServiceCollection AddIdentity(this IServiceCollection services)
    {
        services.AddIdentityCore<User>()
            .AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<RestaurantUserClaimsPrincipalFactory>()
            .AddEntityFrameworkStores<RestaurantDbContext>();

        return services;
    }

    private static IServiceCollection AddAuthorization(this IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(PolicyNames.HasNationality, policy => policy.RequireClaim(AppClaimTypes.Nationality, "Egyptian"));
            options.AddPolicy(PolicyNames.AtLeast18, policy => policy.Requirements.Add(new MinimumAgeRequirement(18)));
            options.AddPolicy(PolicyNames.CanDelete, policy => policy.RequireRole(UserRoles.Owner, UserRoles.Admin));
            options.AddPolicy(PolicyNames.OwnsAtLeastTwoRestaurants, policy => policy.Requirements.Add(new OwnsAtLeastRequirement(2)));
        });

        services.AddTransient<IAuthorizationHandler, MinimumAgeRequirementHandler>();
        services.AddTransient<IAuthorizationHandler, OwnsAtLeastRequirementHandler>();
        services.AddTransient<IRestaurantAuthorizationService, RestaurantAuthorizationService>();

        return services;
    }

    private static IServiceCollection AddBlobStorage(this IServiceCollection services)
    {
        services.ConfigureOptions<BlobStorageSetup>();
        services.AddSingleton<IBlobStorageService, BlobStorageService>();
        return services;
    }
}

