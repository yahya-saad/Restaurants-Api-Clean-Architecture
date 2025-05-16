
namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
{
    private readonly IRestaurantsRepository restaurantRepository;
    private readonly ILogger<DeleteRestaurantCommandHandler> logger;
    public DeleteRestaurantCommandHandler(
        IRestaurantsRepository restaurantRepository,
        ILogger<DeleteRestaurantCommandHandler> logger)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
    }

    public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleting restaurant with ID {request.Id}");
        var restaurant = await restaurantRepository.GetByIdAsync(request.Id, cancellationToken);
        if (restaurant == null)
        {
            logger.LogWarning($"Restaurant with ID {request.Id} not found");
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        }

        await restaurantRepository.DeleteAsync(restaurant);
        logger.LogInformation($"Restaurant with ID {request.Id} deleted successfully");

    }
}
