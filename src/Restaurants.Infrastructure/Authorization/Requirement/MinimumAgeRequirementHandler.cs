using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Common.Identity;

namespace Restaurants.Infrastructure.Authorization.Requirement;
internal class MinimumAgeRequirementHandle(IUserContext userContext) : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var currentUser = userContext.GetCurrentUser();

        if (currentUser.DateOfBirth is null)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (currentUser.DateOfBirth.Value.AddYears(requirement.MinimumAge) <= DateOnly.FromDateTime(DateTime.UtcNow))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}
