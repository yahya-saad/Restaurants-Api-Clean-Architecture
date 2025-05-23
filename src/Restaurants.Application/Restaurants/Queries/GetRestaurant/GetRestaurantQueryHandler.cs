namespace Restaurants.Application.Restaurants.Queries.GetRestaurant;
public class GetRestaurantQueryHandler : IRequestHandler<GetRestaurantQuery, RestaurantDto>
{
    private readonly IRestaurantsRepository restaurantRepository;
    private readonly ILogger<GetRestaurantQueryHandler> logger;
    private readonly IMapper mapper;
    private readonly IBlobStorageService blobStorageService;
    public GetRestaurantQueryHandler(
        IRestaurantsRepository restaurantRepository,
        ILogger<GetRestaurantQueryHandler> logger,
        IMapper mapper,
        IBlobStorageService blobStorageService)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
        this.blobStorageService = blobStorageService;
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
        dto.LogoUrl = blobStorageService.GetUrl(restaurant.LogoUrl!);

        return dto;
    }
}
