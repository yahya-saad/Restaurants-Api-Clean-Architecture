namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    private readonly IRestaurantRepository restaurantRepository;
    private readonly ILogger<GetAllRestaurantsQueryHandler> logger;
    private readonly IMapper mapper;
    public GetAllRestaurantsQueryHandler(
        IRestaurantRepository restaurantRepository,
        ILogger<GetAllRestaurantsQueryHandler> logger,
        IMapper mapper)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all restaurants");
        var restaurants = await restaurantRepository.GetAllAsync(cancellationToken, includeProperties: "Dishes");
        var dtoList = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);
        return dtoList;
    }
}

