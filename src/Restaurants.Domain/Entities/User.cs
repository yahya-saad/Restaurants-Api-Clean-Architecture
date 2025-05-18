using Microsoft.AspNetCore.Identity;

namespace Restaurants.Domain.Entities;
public class User : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Nationality { get; set; }
}
