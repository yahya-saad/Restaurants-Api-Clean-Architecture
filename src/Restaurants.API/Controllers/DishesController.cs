using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteAllDishes;
using Restaurants.Application.Dishes.Commands.DeleteDish;
using Restaurants.Application.Dishes.Commands.UpdateDish;
using Restaurants.Application.Dishes.DTOs;
using Restaurants.Application.Dishes.Queries.GetAllDishes;
using Restaurants.Application.Dishes.Queries.GetDish;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/Restaurant/{restaurantId:int}/[controller]")]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [EndpointSummary("Get all dishes for a restaurant")]
    [EndpointDescription("Retrieves a list of all dishes for a specific restaurant")]
    [ProducesResponseType(typeof(IEnumerable<DishDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllDishes([FromRoute] int restaurantId)
    {
        var dishes = await mediator.Send(new GetAllDishesQuery(restaurantId));
        return Ok(dishes);
    }

    [HttpGet("{dishId:int}")]
    [EndpointSummary("Get a dish by ID")]
    [EndpointDescription("Retrieves a dish by its ID for a specific restaurant")]
    [ProducesResponseType(typeof(DishDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DishDto>> GetDishById(int restaurantId, int dishId)
    {
        var dish = await mediator.Send(new GetDishQuery(restaurantId, dishId));
        return Ok(dish);
    }


    [HttpPost]
    [Authorize(Roles = UserRoles.Owner)]
    [EndpointSummary("Create a new dish")]
    [EndpointDescription("Creates a new dish for a specific restaurant")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishDto dishDto)
    {
        var id = await mediator.Send(new CreateDishCommand(restaurantId, dishDto));
        return CreatedAtAction(nameof(GetDishById), new { restaurantId, dishId = id }, null);
    }

    [HttpDelete]
    [Authorize(Policy = PolicyNames.CanDelete)]
    [EndpointSummary("Delete All dishes")]
    [EndpointDescription("Deletes a dish by its ID for a specific restaurant")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAllDishes(int restaurantId)
    {
        await mediator.Send(new DeleteAllDishesCommand(restaurantId));
        return NoContent();
    }

    [HttpDelete("{dishId:int}")]
    [Authorize(Policy = PolicyNames.CanDelete)]
    [EndpointSummary("Delete a dish by ID")]
    [EndpointDescription("Deletes a dish by its ID for a specific restaurant")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDish(int restaurantId, int dishId)
    {
        await mediator.Send(new DeleteDishCommand(restaurantId, dishId));
        return NoContent();
    }

    [HttpPut("{dishId:int}")]
    [Authorize(Roles = UserRoles.Owner)]
    [EndpointSummary("Update a dish")]
    [EndpointDescription("Updates a dish by ID for a specific restaurant")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDish(int restaurantId, int dishId, [FromBody] UpdateDishDto dto)
    {
        await mediator.Send(new UpdateDishCommand(restaurantId, dishId, dto));
        return NoContent();
    }




}
