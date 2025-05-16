using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;
public class DishesRepository(RestaurantDbContext dbContext) : IDishesRepository
{
    public async Task<IEnumerable<Dish>> GetAllAsync(int restaurantid, CancellationToken cancellationToken = default)
    {
        var dishes = await dbContext.Dishes
            .AsNoTracking()
            .Where(d => d.RestaurantId == restaurantid)
            .ToListAsync(cancellationToken);
        return dishes;
    }

    public Task<Dish?> GetByIdAsync(int restaurantId, int id, CancellationToken cancellationToken = default)
    {
        var dish = dbContext.Dishes
            .AsNoTracking()
        .FirstOrDefaultAsync(d => d.Id == id && d.RestaurantId == restaurantId, cancellationToken);

        return dish;
    }

    public async Task<int> AddAsync(Dish dish, CancellationToken cancellationToken = default)
    {
        await dbContext.Dishes.AddAsync(dish, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return dish.Id;

    }

    public async Task DeleteAsync(Dish dish, CancellationToken cancellationToken = default)
    {
        dbContext.Remove(dish);
        await dbContext.SaveChangesAsync(cancellationToken);

    }


    public async Task UpdateAsync(Dish dish, CancellationToken cancellationToken = default)
    {
        dbContext.Update(dish);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAllByRestaurantIdAsync(int restaurantId, CancellationToken cancellationToken)
    {
        var dishes = await dbContext.Dishes
        .Where(d => d.RestaurantId == restaurantId)
        .ToListAsync(cancellationToken);

        if (!dishes.Any())
            return;

        dbContext.Dishes.RemoveRange(dishes);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
