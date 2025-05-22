using Microsoft.Extensions.Logging;
using Restaurants.Application.Common.Identity;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Services;
public class RestaurantAuthorizationService(ILogger<RestaurantAuthorizationService> logger, IUserContext userContext, IRestaurantsRepository restaurantsRepository) : IRestaurantAuthorizationService
{
    public bool Authorize(Restaurant restaurant, ResourseOperation operation)
    {
        var user = userContext.GetCurrentUser();

        logger.LogInformation("Authorization user {UserEmail}, to {Operation} for Resraurant {RestaurantName}",
            user.Email, operation, restaurant.Name);

        // user can create/read resourse 
        if (operation == ResourseOperation.Create || operation == ResourseOperation.Read)
        {
            logger.LogInformation("create/read operation -- succesfull authorization");
            return true;
        }

        // [Admin] can delete resourse
        if (operation == ResourseOperation.Delete && user.IsInRole(UserRoles.Admin))
        {
            logger.LogInformation("[Admin] delete operation -- succesfull authorization");
            return true;
        }

        // [Owner] can update/delete resourse
        if ((operation == ResourseOperation.Update || operation == ResourseOperation.Delete) &&
            user.Id == restaurant.OwnerId)
        {
            logger.LogInformation("[Owner] update/delete operation -- succesfull authorization");
            return true;

        }

        return false;
    }
}
