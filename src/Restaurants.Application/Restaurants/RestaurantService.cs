using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Restaurants;
internal class RestaurantService(IRestaurantRepository restaurantRepository, ILogger<RestaurantService> logger) : IRestaurantService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all restaurants");

        var restaurants = await restaurantRepository.GetAllAsync(cancellationToken, includeProperties: "Dishes");

        var restaurantDtos = restaurants.Select(r => r.ToDto());

        return restaurantDtos;
    }

    public async Task<RestaurantDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Fetching restaurant with ID {id}");
        var restaurant = await restaurantRepository.GetByIdAsync(id, cancellationToken, includeProperties: "Dishes");

        var dto = restaurant == null ? null : restaurant.ToDto();
        return dto;
    }
}
