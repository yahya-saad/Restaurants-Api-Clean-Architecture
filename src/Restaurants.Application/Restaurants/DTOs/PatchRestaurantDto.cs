namespace Restaurants.Application.Restaurants.DTOs;
public class PatchRestaurantDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Category { get; set; }
    public bool? HasDelivery { get; set; }
}
