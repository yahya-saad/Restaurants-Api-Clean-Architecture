using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using System.Linq.Expressions;

namespace Restaurants.Domain.Interfaces;
public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<(IEnumerable<Restaurant>, int count)> GetAllAsync(
         int pageSize, int pageNumber,
         CancellationToken cancellationToken = default,
         Expression<Func<Restaurant, bool>>? filter = null,
         string? SortBy = null,
        SortDirection? sortDirection = null,
         string? includeProperties = null);

    Task<Restaurant?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default,
        string? includeProperties = null);

    Task<int> AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default);

    Task UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken = default);
    Task DeleteAsync(Restaurant restaurant, CancellationToken cancellationToken = default);
}
