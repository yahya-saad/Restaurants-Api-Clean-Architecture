using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Common.Identity;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Authorization.Requirement;
internal class OwnsAtLeastRequirementHandler(IUserContext userContext, IRestaurantsRepository restaurantsRepository) : AuthorizationHandler<OwnsAtLeastRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnsAtLeastRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        var restaurant = await restaurantsRepository.GetAllAsync();

        var count = restaurant.Count(r => r.OwnerId == currentUser.Id);

        if (count >= requirement.Minimum)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }
    }
}
