namespace Restaurants.Application.Users.Commands.UnassignUserRole;
public record UnassignUserRoleCommand(string Email, string Role) : IRequest;
