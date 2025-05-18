using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
{
    private readonly IRestaurantsRepository restaurantRepository;
    private readonly ILogger<UpdateRestaurantCommandHandler> logger;
    private readonly IMapper mapper;
    private readonly IRestaurantAuthorizationService restaurantAuthorizationService;
    public UpdateRestaurantCommandHandler(IRestaurantsRepository restaurantRepository, ILogger<UpdateRestaurantCommandHandler> logger, IMapper mapper, IRestaurantAuthorizationService restaurantAuthorizationService)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
        this.restaurantAuthorizationService = restaurantAuthorizationService;
    }


    public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updateing restaurant with ID {request.Id}");
        var restaurant = await restaurantRepository.GetByIdAsync(request.Id, cancellationToken);
        if (restaurant == null)
        {
            logger.LogWarning($"Restaurant with ID {request.Id} not found");
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourseOperation.Delete))
            throw new ForbiddenException();

        mapper.Map(request.Dto, restaurant);

        await restaurantRepository.UpdateAsync(restaurant, cancellationToken);
        logger.LogInformation($"Restaurant with ID {request.Id} updated successfully");
    }
}
