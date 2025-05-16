namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
public record UpdateRestaurantCommand(int Id, UpdateRestaurantDto Dto) : IRequest;
