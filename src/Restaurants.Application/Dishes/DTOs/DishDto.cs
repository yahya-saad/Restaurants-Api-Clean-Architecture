namespace Restaurants.Application.Dishes.DTOs;
public class DishDto
{
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
}
