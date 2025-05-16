using Restaurants.Application.Dishes.DTOs;

namespace Restaurants.Application.Dishes.Commands.CreateDish;
public record CreateDishCommand(int RestaurantId, CreateDishDto DishDto) : IRequest<int>;
