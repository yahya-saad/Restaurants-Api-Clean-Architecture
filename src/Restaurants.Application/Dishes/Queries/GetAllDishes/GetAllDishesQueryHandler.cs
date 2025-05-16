using Restaurants.Application.Dishes.DTOs;

namespace Restaurants.Application.Dishes.Queries.GetAllDishes;
public class GetAllDishesQueryHandler : IRequestHandler<GetAllDishesQuery, IEnumerable<DishDto>>
{
    private readonly ILogger<GetAllDishesQueryHandler> _logger;
    private readonly IRestaurantsRepository _restaurantRepository;
    private readonly IDishesRepository _dishesRepository;
    private readonly IMapper _mapper;
    public GetAllDishesQueryHandler(ILogger<GetAllDishesQueryHandler> logger, IRestaurantsRepository restaurantRepository, IDishesRepository dishesRepository, IMapper mapper)
    {
        _logger = logger;
        _restaurantRepository = restaurantRepository;
        _dishesRepository = dishesRepository;
        _mapper = mapper;
    }


    public async Task<IEnumerable<DishDto>> Handle(GetAllDishesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all dishes for restaurant {RestaurantId}", request.RestaurantId);
        var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId, includeProperties: "Dishes");
        if (restaurant == null)
        {
            _logger.LogWarning("Restaurant with ID {RestaurantId} not found", request.RestaurantId);
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }
        /*   var dishes = await _dishesRepository.GetAllAsync(request.RestaurantId, cancellationToken);
           var dishDtos = _mapper.Map<IEnumerable<DishDto>>(dishes);*/
        var dishDtos = _mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);
        return dishDtos;
    }
}
