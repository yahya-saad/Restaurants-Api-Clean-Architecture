namespace Restaurants.Application.Common.Identity;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}