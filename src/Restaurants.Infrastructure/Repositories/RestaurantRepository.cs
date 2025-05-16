using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;
internal class RestaurantRepository(RestaurantDbContext _dbContext) : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllAsync(
        CancellationToken cancellationToken = default,
        string? includeProperties = null)
    {
        IQueryable<Restaurant> query = _dbContext.Restaurants;

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
        }
        return await query.ToListAsync(cancellationToken);
    }
    public async Task<Restaurant?> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default,
        string? includeProperties = null)
    {
        IQueryable<Restaurant> query = _dbContext.Restaurants;

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
        }
        return await query.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<int> AddAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        await _dbContext.Restaurants.AddAsync(restaurant, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return restaurant.Id;
    }

    public async Task DeleteAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        _dbContext.Remove(restaurant);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        _dbContext.Update(restaurant);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}

