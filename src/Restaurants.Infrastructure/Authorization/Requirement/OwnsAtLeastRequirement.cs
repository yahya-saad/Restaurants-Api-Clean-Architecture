using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Infrastructure.Authorization.Requirement;
public class OwnsAtLeastRequirement(int minimum) : IAuthorizationRequirement
{
    public int Minimum { get; set; } = minimum;
}
