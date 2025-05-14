using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.Application.Restaurants;
public interface IRestaurantService
{
    Task<IEnumerable<RestaurantDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<RestaurantDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
}