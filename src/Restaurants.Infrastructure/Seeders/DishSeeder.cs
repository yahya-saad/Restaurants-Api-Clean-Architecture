using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;
internal class DishSeeder(RestaurantDbContext context) : ISeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await context.Database.CanConnectAsync(cancellationToken) &&
            !await context.Dishes.AnyAsync(cancellationToken))
        {
            var dishes = GetDishes();
            await context.Dishes.AddRangeAsync(dishes, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    private IEnumerable<Dish> GetDishes()
    {
        var dishes = new List<Dish>
        {
            new Dish
            {
                Name = "Margherita Pizza",
                Description = "Classic pizza with fresh mozzarella, tomatoes, and basil.",
                Price = 12.99m,
                RestaurantId = 1
            },
            new Dish
            {
                Name = "California Roll",
                Description = "Sushi roll with crab, avocado, and cucumber.",
                Price = 8.99m,
                RestaurantId = 2
            },
            new Dish
            {
                Name = "Butter Chicken",
                Description = "Tender chicken in a creamy tomato sauce.",
                Price = 14.99m,
                RestaurantId = 3
            },
            new Dish
            {
                Name = "Margherita Pizza",
                Description = "Tomato, mozzarella, basil",
                Price = 10.99m,
                RestaurantId = 4
            },
            new Dish
            {
                Name = "Spaghetti Carbonara",
                Description = "Classic Italian pasta",
                Price = 12.99m,
                RestaurantId = 4
            }
        };

        return dishes;
    }
}
