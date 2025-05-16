using Restaurants.Application.Dishes.DTOs;

namespace Restaurants.Application.Dishes.Queries.GetDish;
public record GetDishQuery(int RestaurantId, int DishId) : IRequest<DishDto>;
