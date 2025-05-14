using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;
internal class RestaurantSeeder : ISeeder
{
    private readonly RestaurantDbContext _context;
    public RestaurantSeeder(RestaurantDbContext context)
    {
        _context = context;
    }
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _context.Database.CanConnectAsync(cancellationToken) &&
           !await _context.Restaurants.AnyAsync(cancellationToken))
        {
            var restaurant = GetRestaurants();
            await _context.Restaurants.AddRangeAsync(restaurant, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }


    private IEnumerable<Restaurant> GetRestaurants()
    {
        List<Restaurant> restaurants = new()
    {
        new Restaurant
        {
            Name = "The Italian Kitchen",
            Description = "Authentic Italian cuisine with a modern twist.",
            Category = "Italian",
            HasDelivery = true,
            ContractEmail = "contact@italiankitchen.com",
            ContractNumber = "123-456-7890",
            Address = new Address
            {
                Street = "123 Pasta Lane",
                City = "New York",
                PostalCode = "10001"
            }
        },
        new Restaurant
        {
            Name = "Sushi World",
            Description = "Fresh sushi and sashimi prepared by expert chefs.",
            Category = "Japanese",
            HasDelivery = false,
            ContractEmail = "info@sushiworld.jp",
            ContractNumber = "212-555-1212",
            Address = new Address
            {
                Street = "456 Tokyo Ave",
                City = "San Francisco",
                PostalCode = "94103"
            }
        },
        new Restaurant
        {
            Name = "Spice of India",
            Description = "Traditional Indian dishes with rich spices and flavors.",
            Category = "Indian",
            HasDelivery = true,
            ContractEmail = "hello@spiceofindia.in",
            ContractNumber = "987-654-3210",
            Address = new Address
            {
                Street = "789 Curry Blvd",
                City = "Chicago",
                PostalCode = "60614"
            }
        },
        new Restaurant
        {
            Name = "Burger Haven",
            Description = "Gourmet burgers with locally sourced ingredients.",
            Category = "American",
            HasDelivery = true,
            ContractEmail = "orders@burgerhaven.com",
            ContractNumber = "555-789-1234",
            Address = new Address
            {
                Street = "321 Grill St",
                City = "Austin",
                PostalCode = "73301"
            }
        },
        new Restaurant
        {
            Name = "Taco Fiesta",
            Description = "Vibrant Mexican food with a street-style feel.",
            Category = "Mexican",
            HasDelivery = false,
            ContractEmail = "support@tacofiesta.mx",
            ContractNumber = "888-555-6789",
            Address = new Address
            {
                Street = "159 Salsa Rd",
                City = "Los Angeles",
                PostalCode = "90001"
            }
        },
        new Restaurant
        {
            Name = "Green Garden",
            Description = "Healthy plant-based meals and smoothies.",
            Category = "Vegan",
            HasDelivery = true,
            ContractEmail = "hi@greengarden.org",
            ContractNumber = "444-321-6789",
            Address = new Address
            {
                Street = "753 Veggie Way",
                City = "Seattle",
                PostalCode = "98101"
            }
        },
        new Restaurant
        {
            Name = "Le Petit Bistro",
            Description = "Cozy French bistro with classic dishes and wine.",
            Category = "French",
            HasDelivery = false,
            ContractEmail = "reservations@lepetitbistro.fr",
            ContractNumber = "321-654-9870",
            Address = new Address
            {
                Street = "951 Paris Ln",
                City = "Boston",
                PostalCode = "02118"
            }
        },
        new Restaurant
        {
            Name = "Dragon Wok",
            Description = "Fast and flavorful Chinese takeout and delivery.",
            Category = "Chinese",
            HasDelivery = true,
            ContractEmail = "contact@dragonwok.cn",
            ContractNumber = "800-123-4567",
            Address = new Address
            {
                Street = "369 Wok St",
                City = "Houston",
                PostalCode = "77002"
            }
        },
        new Restaurant
        {
            Name = "The Greek Table",
            Description = "Mediterranean classics like gyros, souvlaki, and hummus.",
            Category = "Greek",
            HasDelivery = true,
            ContractEmail = "info@greektable.gr",
            ContractNumber = "999-333-2211",
            Address = new Address
            {
                Street = "258 Athens Blvd",
                City = "Phoenix",
                PostalCode = "85001"
            }
        },
        new Restaurant
        {
            Name = "BBQ Barn",
            Description = "Slow-smoked meats and homemade sauces.",
            Category = "BBQ",
            HasDelivery = false,
            ContractEmail = "bbq@barnsmokehouse.com",
            ContractNumber = "666-777-8888",
            Address = new Address
            {
                Street = "147 Smokehouse Rd",
                City = "Dallas",
                PostalCode = "75201"
            }
        }
    };

        return restaurants;
    }


}
