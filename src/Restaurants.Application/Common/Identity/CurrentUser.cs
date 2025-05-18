namespace Restaurants.Application.Common.Identity;
public record CurrentUser(string Id, string Email, IEnumerable<string> Roles)
{
    public bool isInRole(string role) => Roles.Contains(role);
}