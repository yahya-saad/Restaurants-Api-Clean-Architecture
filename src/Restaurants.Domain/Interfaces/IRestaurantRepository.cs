using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Interfaces;
public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync(
        CancellationToken cancellationToken = default,
        string? includeProperties = null);

    Task<Restaurant?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default,
        string? includeProperties = null);

    Task<int> AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default);

    /*  Task UpdateAsync(Restaurant restaurant);
     Task DeleteAsync(int id);*/
}
