using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Interfaces;
public interface IDishesRepository
{
    Task<IEnumerable<Dish>> GetAllAsync(int restaurantId, CancellationToken cancellationToken = default);
    Task<Dish?> GetByIdAsync(int restaurantId, int id, CancellationToken cancellationToken = default);
    Task<int> AddAsync(Dish dish, CancellationToken cancellationToken = default);
    Task UpdateAsync(Dish dish, CancellationToken cancellationToken = default);
    Task DeleteAsync(Dish dish, CancellationToken cancellationToken = default);
    Task DeleteAllByRestaurantIdAsync(int restaurantId, CancellationToken cancellationToken);
}
