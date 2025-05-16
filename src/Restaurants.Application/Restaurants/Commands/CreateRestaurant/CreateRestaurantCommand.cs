namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;
public record CreateRestaurantCommand(CreateRestaurantDto Dto) : IRequest<int>;

