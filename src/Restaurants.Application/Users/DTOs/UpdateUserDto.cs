namespace Restaurants.Application.Users.DTOs;
public class UpdateUserDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly DateOfBirth { get; set; }
    public string Nationality { get; set; } = default!;
}
