using Restaurants.Application.Dishes.DTOs;

namespace Restaurants.Application.Dishes.Commands.UpdateDish;
public record UpdateDishCommand(int RestaurantId, int DishId, UpdateDishDto DishDto) : IRequest;
