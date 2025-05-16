namespace Restaurants.Application.Dishes.Commands.DeleteDish;
public record DeleteDishCommand(int RestaurantId, int DishId) : IRequest;
