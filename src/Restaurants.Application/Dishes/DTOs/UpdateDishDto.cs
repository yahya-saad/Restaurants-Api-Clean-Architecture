namespace Restaurants.Application.Dishes.DTOs;
public class UpdateDishDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int KiloCalories { get; set; }
}
