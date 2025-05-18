using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.PatchRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurant;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    [EndpointSummary("Get all restaurants")]
    [EndpointDescription("Retrieves a list of all restaurants")]
    [ProducesResponseType(typeof(IEnumerable<RestaurantDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [EndpointSummary("Get restaurant by ID")]
    [EndpointDescription("Retrieves a restaurant by its ID")]
    [ProducesResponseType(typeof(RestaurantDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantQuery(id));
        return Ok(restaurant);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    [EndpointSummary("Create a new restaurant")]
    [EndpointDescription("Creates a new restaurant")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantDto restaurantDto)
    {
        var id = await mediator.Send(new CreateRestaurantCommand(restaurantDto));
        return CreatedAtAction(nameof(GetById), new { id = id }, null);
    }

    [HttpDelete("{id:int}")]
    [EndpointSummary("Delete a restaurant")]
    [EndpointDescription("Deletes a restaurant by its ID")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await mediator.Send(new DeleteRestaurantCommand(id));
        return NoContent();
    }

    [HttpPatch("{id:int}")]
    [EndpointSummary("Partially update a restaurant")]
    [EndpointDescription("<b>Partially</b> updates a restaurant by its ID")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PartialUpdate(int id, [FromBody] JsonPatchDocument<PatchRestaurantDto> patchDocument)
    {
        await mediator.Send(new PatchRestaurantCommand(id, patchDocument));
        return NoContent();
    }


    [HttpPut("{id:int}")]
    [EndpointSummary("Update a restaurant")]
    [EndpointDescription("Updates a restaurant by its ID")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateRestaurantDto updateRestaurantDto)
    {
        await mediator.Send(new UpdateRestaurantCommand(id, updateRestaurantDto));
        return NoContent();
    }


}
