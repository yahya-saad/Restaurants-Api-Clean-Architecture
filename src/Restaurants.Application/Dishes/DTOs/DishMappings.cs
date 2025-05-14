using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dishes.DTOs;
internal static class DishMappings
{
    public static DishDto ToDto(this Dish dish)
    {
        return new DishDto
        {
            Name = dish.Name,
            Description = dish.Description,
            Price = dish.Price
        };
    }
}
