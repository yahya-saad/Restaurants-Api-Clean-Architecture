
using Microsoft.AspNetCore.Identity;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;
public class UnassignUserRoleCommandHandler : IRequestHandler<UnassignUserRoleCommand>
{
    private readonly ILogger<UnassignUserRoleCommandHandler> _logger;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UnassignUserRoleCommandHandler(ILogger<UnassignUserRoleCommandHandler> logger, IUserStore<User> userStore, IRoleStore<IdentityRole> roleStore, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
    }


    public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unassigning user role {@Request}", request);

        var user = await _userManager.FindByEmailAsync(request.Email) ??
                        throw new NotFoundException(nameof(User), request.Email);

        var role = await _roleManager.FindByNameAsync(request.Role) ??
                        throw new NotFoundException(nameof(IdentityRole), request.Role);

        await _userManager.RemoveFromRoleAsync(user, role.Name!);

    }
}
