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
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all restaurants")]
    [EndpointDescription("Retrieves a list of all restaurants")]
    [ProducesResponseType(typeof(IEnumerable<RestaurantDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllRestaurantsQuery query)
    {
        if (ModelState.IsValid == false)
        {
            return BadRequest(ModelState);
        }

        var restaurants = await mediator.Send(query);
        return Ok(restaurants);
    }

    [HttpGet("{id:int}")]
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
    [Authorize()]
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
    [Authorize(Policy = PolicyNames.CanDelete)]
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
    [Authorize(Roles = UserRoles.Owner)]
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
    [Authorize(Roles = UserRoles.Owner)]
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
