
using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Common.Identity;

namespace Restaurants.Application.Users.Commands.UpdateUser;
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    private readonly IUserContext _userContext;
    private readonly IUserStore<User> _userStore;

    public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, IUserContext userContext, IUserStore<User> userStore)
    {
        _logger = logger;
        _userContext = userContext;
        _userStore = userStore;
    }


    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {

        var user = _userContext.GetCurrentUser();

        _logger.LogInformation("Handling UpdateUserCommand for user with ID: {UserId}", user!.Id);

        var dbuser = await _userStore.FindByIdAsync(user.Id, cancellationToken);

        if (dbuser == null)
        {
            _logger.LogWarning("User with ID: {UserId} not found", user!.Id);
            throw new NotFoundException(nameof(user), user!.Id);
        }

        dbuser.FirstName = request.Dto.FirstName;
        dbuser.LastName = request.Dto.LastName;
        dbuser.DateOfBirth = request.Dto.DateOfBirth;
        dbuser.Nationality = request.Dto.Nationality;

        await _userStore.UpdateAsync(dbuser, cancellationToken);
    }
}
