using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirement;
internal class OwnsAtLeastRequirement(int minimum) : IAuthorizationRequirement
{
    public int Minimum { get; set; } = minimum;
}
