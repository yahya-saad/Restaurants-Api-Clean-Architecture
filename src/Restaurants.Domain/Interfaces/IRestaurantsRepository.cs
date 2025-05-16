using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Interfaces;
public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync(
        CancellationToken cancellationToken = default,
        string? includeProperties = null);

    Task<Restaurant?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default,
        string? includeProperties = null);

    Task<int> AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default);

    Task UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken = default);
    Task DeleteAsync(Restaurant restaurant, CancellationToken cancellationToken = default);
}
