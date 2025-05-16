namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;
public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, int>
{
    private readonly IRestaurantsRepository restaurantRepository;
    private readonly ILogger<CreateRestaurantCommandHandler> logger;
    private readonly IMapper mapper;
    public CreateRestaurantCommandHandler(
        IRestaurantsRepository restaurantRepository,
        ILogger<CreateRestaurantCommandHandler> logger,
        IMapper mapper)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Creating restaurant with name {request.Dto.Name}");
        var restaurant = mapper.Map<Restaurant>(request.Dto);
        int id = await restaurantRepository.AddAsync(restaurant, cancellationToken);
        return id;
    }
}
