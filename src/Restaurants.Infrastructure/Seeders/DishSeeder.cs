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
            var italian = await context.Restaurants.FirstOrDefaultAsync(r => r.Name == "The Italian Kitchen", cancellationToken);
            var sushi = await context.Restaurants.FirstOrDefaultAsync(r => r.Name == "Sushi World", cancellationToken);
            var indian = await context.Restaurants.FirstOrDefaultAsync(r => r.Name == "Spice of India", cancellationToken);
            var american = await context.Restaurants.FirstOrDefaultAsync(r => r.Name == "Burger Haven", cancellationToken);

            var dishes = new List<Dish>
            {
                new Dish
                {
                    Name = "Margherita Pizza",
                    Description = "Classic pizza with fresh mozzarella, tomatoes, and basil.",
                    Price = 12.99m,
                    RestaurantId = italian?.Id ?? 0
                },
                new Dish
                {
                    Name = "California Roll",
                    Description = "Sushi roll with crab, avocado, and cucumber.",
                    Price = 8.99m,
                    RestaurantId = sushi?.Id ?? 0
                },
                new Dish
                {
                    Name = "Butter Chicken",
                    Description = "Tender chicken in a creamy tomato sauce.",
                    Price = 14.99m,
                    RestaurantId = indian?.Id ?? 0
                },
                new Dish
                {
                    Name = "Margherita Pizza",
                    Description = "Tomato, mozzarella, basil",
                    Price = 10.99m,
                    RestaurantId = american?.Id ?? 0
                }
            };

            await context.Dishes.AddRangeAsync(dishes.Where(d => d.RestaurantId != 0), cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
