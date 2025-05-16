
namespace Restaurants.Application.Dishes.Commands.DeleteAllDishes;
public class DeleteAllDishesCommandHandler : IRequestHandler<DeleteAllDishesCommand>
{
    private readonly IDishesRepository _dishesRepository;
    private readonly IRestaurantsRepository _restaurantsRepository;
    private readonly ILogger<DeleteAllDishesCommandHandler> _logger;

    public DeleteAllDishesCommandHandler(IDishesRepository dishesRepository, ILogger<DeleteAllDishesCommandHandler> logger, IRestaurantsRepository restaurantsRepository)
    {
        _dishesRepository = dishesRepository;
        _logger = logger;
        _restaurantsRepository = restaurantsRepository;
    }

    public async Task Handle(DeleteAllDishesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting all dishes for restaurant {RestaurantId}", request.RestaurantId);

        var restaurant = await _restaurantsRepository.GetByIdAsync(request.RestaurantId, cancellationToken: cancellationToken);
        if (restaurant == null)
        {
            _logger.LogWarning("Restaurant with ID {RestaurantId} not found", request.RestaurantId);
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        await _dishesRepository.DeleteAllByRestaurantIdAsync(request.RestaurantId, cancellationToken);
        _logger.LogInformation("All dishes for restaurant {RestaurantId} deleted", request.RestaurantId);
    }
}
