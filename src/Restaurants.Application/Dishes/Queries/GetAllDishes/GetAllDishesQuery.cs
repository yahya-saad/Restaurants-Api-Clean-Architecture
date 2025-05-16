using Restaurants.Application.Dishes.DTOs;

namespace Restaurants.Application.Dishes.Queries.GetAllDishes;
public record GetAllDishesQuery(int RestaurantId) : IRequest<IEnumerable<DishDto>>;
