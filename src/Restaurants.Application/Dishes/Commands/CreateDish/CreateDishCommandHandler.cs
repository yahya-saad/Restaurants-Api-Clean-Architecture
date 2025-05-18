
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Dishes.Commands.CreateDish;
public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, int>
{
    private readonly ILogger<CreateDishCommandHandler> _logger;
    private readonly IRestaurantsRepository _restaurantRepository;
    private readonly IDishesRepository _dishesRepository;
    private readonly IMapper _mapper;
    private readonly IRestaurantAuthorizationService restaurantAuthorizationService;

    public CreateDishCommandHandler(ILogger<CreateDishCommandHandler> logger, IRestaurantsRepository restaurantRepository, IDishesRepository dishesRepository, IMapper mapper, IRestaurantAuthorizationService restaurantAuthorizationService)
    {
        _logger = logger;
        _restaurantRepository = restaurantRepository;
        _dishesRepository = dishesRepository;
        _mapper = mapper;
        this.restaurantAuthorizationService = restaurantAuthorizationService;
    }

    async Task<int> IRequestHandler<CreateDishCommand, int>.Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating a new dish:{@DishRequest} for restaurant {RestaurantId}", request.DishDto, request.RestaurantId);

        var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant == null)
        {
            _logger.LogWarning("Restaurant with ID {RestaurantId} not found", request.RestaurantId);
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourseOperation.Update))
            throw new ForbiddenException();

        var dish = _mapper.Map<Dish>(request.DishDto);
        dish.RestaurantId = request.RestaurantId;

        await _dishesRepository.AddAsync(dish, cancellationToken);
        _logger.LogInformation("Dish created with ID {DishId}", dish.Id);
        return dish.Id;
    }
}
