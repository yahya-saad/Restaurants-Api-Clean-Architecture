using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Restaurants;
internal class RestaurantService(
    IRestaurantRepository restaurantRepository,
    ILogger<RestaurantService> logger,
    IMapper mapper) : IRestaurantService
{


    public async Task<IEnumerable<RestaurantDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all restaurants");

        var restaurants = await restaurantRepository.GetAllAsync(cancellationToken, includeProperties: "Dishes");

        var restaurantDtos = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        return restaurantDtos;
    }

    public async Task<RestaurantDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Fetching restaurant with ID {id}");
        var restaurant = await restaurantRepository.GetByIdAsync(id, cancellationToken, includeProperties: "Dishes");

        var dto = mapper.Map<RestaurantDto?>(restaurant);
        return dto;
    }

    public async Task<int> CreateAsync(CreateResaturantDto restaurantDto, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Creating restaurant with name {restaurantDto.Name}");
        var restaurant = mapper.Map<Restaurant>(restaurantDto);
        int id = await restaurantRepository.AddAsync(restaurant, cancellationToken);
        return id;
    }
}
