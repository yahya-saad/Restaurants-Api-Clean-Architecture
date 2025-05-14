using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController(IRestaurantService restaurantService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all restaurants")]
    [EndpointDescription("Retrieves a list of all restaurants")]

    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var restaurants = await restaurantService.GetAllAsync(cancellationToken);
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get restaurant by ID")]
    [EndpointDescription("Retrieves a restaurant by its ID")]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var restaurant = await restaurantService.GetByIdAsync(id, cancellationToken);
        if (restaurant == null)
        {
            return NotFound();
        }
        return Ok(restaurant);
    }
}
