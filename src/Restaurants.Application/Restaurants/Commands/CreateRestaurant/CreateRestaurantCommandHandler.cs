using Restaurants.Application.Common.Identity;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;
public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
{
    private readonly IRestaurantsRepository restaurantRepository;
    private readonly ILogger<CreateRestaurantCommandHandler> logger;
    private readonly IMapper mapper;
    private readonly IUserContext userContext;
    public CreateRestaurantCommandHandler(
        IRestaurantsRepository restaurantRepository,
        ILogger<CreateRestaurantCommandHandler> logger,
        IMapper mapper,
        IUserContext userContext)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
        this.userContext = userContext;
    }

    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation(" {UserEmail} [{UserId}] - Creating a new restaurant {@Restaurant}",
            currentUser.Email,
            currentUser.Id,
            request.Dto);

        var restaurant = mapper.Map<Restaurant>(request.Dto);
        restaurant.OwnerId = currentUser.Id;
        int id = await restaurantRepository.AddAsync(restaurant, cancellationToken);
        return id;
    }
}
