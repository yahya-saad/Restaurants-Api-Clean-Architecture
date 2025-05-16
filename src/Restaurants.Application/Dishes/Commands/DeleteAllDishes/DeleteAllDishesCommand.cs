namespace Restaurants.Application.Dishes.Commands.DeleteAllDishes;
public record DeleteAllDishesCommand(int RestaurantId) : IRequest;
