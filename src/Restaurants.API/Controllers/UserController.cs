using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Users.Commands.AssignUserRole;
using Restaurants.Application.Users.Commands.UnassignUserRole;
using Restaurants.Application.Users.Commands.UpdateUser;
using Restaurants.Application.Users.DTOs;
using Restaurants.Application.Users.Queries.GetCurrentUser;
using Restaurants.Domain.Constants;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("me")]
    [Authorize]
    [EndpointSummary("Get current user")]
    [EndpointDescription("Retrieves the current user information")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        var user = mediator.Send(new GetCurrentUserQuery()).Result;
        return Ok(new
        {
            UserId = user.Id,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            FullName = $"{user.FirstName} {user.LastName}",
            Nationality = user.Nationality
        });
    }

    [HttpPatch]
    [Authorize]
    [EndpointSummary("Update current user")]
    [EndpointDescription("Updates the current user information")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserDto userDto)
    {
        await mediator.Send(new UpdateUserCommand(userDto));
        return NoContent();
    }

    [HttpPost("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    [EndpointSummary("[Admin] Assign user role")]
    [EndpointDescription("Assigns a role to a user - <b> Admin </b>")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignUserRole([FromBody] AssignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("userRole")]
    [Authorize(Roles = UserRoles.Admin)]
    [EndpointSummary("[Admin] Unassign user role")]
    [EndpointDescription("Unassign a role to a user - <b> Admin </b>")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UnassignUserRole([FromBody] UnassignUserRoleCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
}
