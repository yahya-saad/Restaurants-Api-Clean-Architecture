using Microsoft.AspNetCore.Identity;
using Restaurants.Application.Common.Identity;

namespace Restaurants.Application.Users.Queries.GetCurrentUser;
public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, User>
{
    private readonly IUserContext _userContext;
    private readonly IUserStore<User> _userStore;
    private readonly ILogger<GetCurrentUserQueryHandler> _logger;
    public GetCurrentUserQueryHandler(IUserContext userContext, IUserStore<User> userStore, ILogger<GetCurrentUserQueryHandler> logger)
    {
        _userContext = userContext;
        _userStore = userStore;
        _logger = logger;
    }
    public async Task<User> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var currentUser = _userContext.GetCurrentUser();

        _logger.LogInformation("Fetching user details for {UserId}", currentUser.Id);

        var user = await _userStore.FindByIdAsync(currentUser.Id, cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(User), currentUser.Id.ToString());
        }

        return user;
    }
}

