using Restaurants.Application.Dishes.DTOs;

namespace Restaurants.Application.Dishes.Queries.GetDish;
public class GetDishQueryHandler : IRequestHandler<GetDishQuery, DishDto>
{
    private readonly ILogger<GetDishQueryHandler> _logger;
    private readonly IRestaurantsRepository _restaurantRepository;
    private readonly IDishesRepository _dishesRepository;
    private readonly IMapper _mapper;

    public GetDishQueryHandler(ILogger<GetDishQueryHandler> logger, IRestaurantsRepository restaurantRepository, IDishesRepository dishesRepository, IMapper mapper)
    {
        _logger = logger;
        _restaurantRepository = restaurantRepository;
        _dishesRepository = dishesRepository;
        _mapper = mapper;
    }

    public async Task<DishDto> Handle(GetDishQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting dish with ID {DishId} for restaurant {RestaurantId}", request.DishId, request.RestaurantId);
        var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            _logger.LogWarning("Restaurant with ID {RestaurantId} not found", request.RestaurantId);
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        var dish = await _dishesRepository.GetByIdAsync(request.RestaurantId, request.DishId, cancellationToken);
        if (dish == null)
        {
            _logger.LogWarning("Dish with ID {DishId} not found", request.DishId);
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        }

        var dishDto = _mapper.Map<DishDto>(dish);
        return dishDto;
    }
}
