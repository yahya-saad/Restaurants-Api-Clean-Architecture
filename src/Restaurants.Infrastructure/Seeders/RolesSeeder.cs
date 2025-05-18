
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;
internal class RolesSeeder(RestaurantDbContext dbContext) : ISeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await dbContext.Database.CanConnectAsync(cancellationToken) &&
            !await dbContext.Roles.AnyAsync(cancellationToken))
        {
            var roles = GetRoles();
            await dbContext.Roles.AddRangeAsync(roles);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        return new List<IdentityRole>
        {
            new IdentityRole
            {
                Name = UserRoles.Admin,
                NormalizedName = UserRoles.Admin.ToUpper()
            },
            new IdentityRole
            {
                Name = UserRoles.User,
                NormalizedName = UserRoles.User.ToUpper()
            },
            new IdentityRole
            {
                Name = UserRoles.Owner,
                NormalizedName = UserRoles.Owner.ToUpper()
            }
        };
    }
}
