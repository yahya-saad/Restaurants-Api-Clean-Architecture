using Restaurants.Application.Dishes.DTOs;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.DTOs;
internal static class RestaurantMappings
{
    public static RestaurantDto ToDto(this Restaurant restaurant)
    {
        return new RestaurantDto
        {
            Id = restaurant.Id,
            Name = restaurant.Name,
            Description = restaurant.Description,
            Category = restaurant.Category,
            HasDelivery = restaurant.HasDelivery,
            Street = restaurant.Address?.Street,
            City = restaurant.Address?.City,
            PostalCode = restaurant.Address?.PostalCode,
            Dishes = restaurant.Dishes.Select(d => d.ToDto()).ToList()
        };
    }
}
