using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantController(IRestaurantService restaurantService) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all restaurants")]
    [EndpointDescription("Retrieves a list of all restaurants")]
    [ProducesResponseType(typeof(IEnumerable<RestaurantDto>), StatusCodes.Status200OK)]

    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var restaurants = await restaurantService.GetAllAsync(cancellationToken);
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    [EndpointSummary("Get restaurant by ID")]
    [EndpointDescription("Retrieves a restaurant by its ID")]
    [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var restaurant = await restaurantService.GetByIdAsync(id, cancellationToken);
        if (restaurant == null)
        {
            return NotFound();
        }
        return Ok(restaurant);
    }

    [HttpPost]
    [EndpointSummary("Create a new restaurant")]
    [EndpointDescription("Creates a new restaurant")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateResaturantDto restaurantDto,
        CancellationToken cancellationToken,
        IValidator<CreateResaturantDto> validator)
    {
        await validator.ValidateAndThrowAsync(restaurantDto, cancellationToken);
        var id = await restaurantService.CreateAsync(restaurantDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = id }, null);
    }
}
