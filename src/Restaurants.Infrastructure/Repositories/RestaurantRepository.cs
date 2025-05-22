using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Persistence;
using Restaurants.Infrastructure.Persistence.Extensions;
using System.Linq.Expressions;

namespace Restaurants.Infrastructure.Repositories;
internal class RestaurantRepository(RestaurantDbContext _dbContext) : IRestaurantsRepository
{

    public async Task<IEnumerable<Restaurant>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Restaurants
          .AsNoTracking()
          .ToListAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Restaurant>, int count)> GetAllAsync(
        int pageSize, int pageNumber,
        CancellationToken cancellationToken = default,
        Expression<Func<Restaurant, bool>>? filter = null,
        string? SortBy = null,
        SortDirection? sortDirection = null,
        string? includeProperties = null)
    {
        IQueryable<Restaurant> query = _dbContext.Restaurants;

        // Apply includes
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
        }

        // Apply filter
        if (filter != null)
        {
            query = query.Where(filter);
        }

        var totalCount = await query.CountAsync();

        // sorting
        if (!string.IsNullOrEmpty(SortBy))
        {
            query = query.ApplySorting(SortBy, sortDirection);
        }

        // Paginate
        query = query.ApplyPagination(pageNumber, pageSize);

        var items = await query.ToListAsync();

        return (items, totalCount);
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

