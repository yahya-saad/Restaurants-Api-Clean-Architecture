namespace Restaurants.Application.Restaurants.Queries.GetRestaurant;
public class GetRestaurantQueryHandler : IRequestHandler<GetRestaurantQuery, RestaurantDto>
{
    private readonly IRestaurantRepository restaurantRepository;
    private readonly ILogger<GetRestaurantQueryHandler> logger;
    private readonly IMapper mapper;
    public GetRestaurantQueryHandler(
        IRestaurantRepository restaurantRepository,
        ILogger<GetRestaurantQueryHandler> logger,
        IMapper mapper)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
    }
    public async Task<RestaurantDto> Handle(GetRestaurantQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Fetching restaurant with ID {request.Id}");
        var restaurant = await restaurantRepository.GetByIdAsync(request.Id, cancellationToken, includeProperties: "Dishes");

        if (restaurant is null)
        {
            logger.LogWarning($"Restaurant with ID {request.Id} not found");
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        }
        var dto = mapper.Map<RestaurantDto?>(restaurant);
        return dto;
    }
}
