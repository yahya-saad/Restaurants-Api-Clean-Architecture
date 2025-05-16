
namespace Restaurants.Application.Dishes.Commands.DeleteDish;
public class DeleteDishCommandHandler : IRequestHandler<DeleteDishCommand>
{
    private readonly ILogger<DeleteDishCommandHandler> _logger;
    private readonly IRestaurantsRepository _restaurantRepository;
    private readonly IDishesRepository _dishesRepository;

    public DeleteDishCommandHandler(ILogger<DeleteDishCommandHandler> logger, IRestaurantsRepository restaurantRepository, IDishesRepository dishesRepository)
    {
        _logger = logger;
        _restaurantRepository = restaurantRepository;
        _dishesRepository = dishesRepository;
    }


    public async Task Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting dish with ID {DishId} for restaurant {RestaurantId}", request.DishId, request.RestaurantId);
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

        await _dishesRepository.DeleteAsync(dish, cancellationToken);
        _logger.LogInformation("Dish with ID {DishId} deleted", request.DishId);

    }
}