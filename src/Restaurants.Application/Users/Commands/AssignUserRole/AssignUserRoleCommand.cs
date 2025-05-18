namespace Restaurants.Application.Users.Commands.AssignUserRole;
public record AssignUserRoleCommand(string Email, string Role) : IRequest;
