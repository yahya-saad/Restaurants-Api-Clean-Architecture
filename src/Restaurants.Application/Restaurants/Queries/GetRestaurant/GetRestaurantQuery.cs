namespace Restaurants.Application.Restaurants.Queries.GetRestaurant;

public record GetRestaurantQuery(int Id) : IRequest<RestaurantDto>;
