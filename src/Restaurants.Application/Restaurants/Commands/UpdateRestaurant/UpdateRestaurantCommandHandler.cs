namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
{
    private readonly IRestaurantRepository restaurantRepository;
    private readonly ILogger<UpdateRestaurantCommandHandler> logger;
    private readonly IMapper mapper;
    public UpdateRestaurantCommandHandler(IRestaurantRepository restaurantRepository, ILogger<UpdateRestaurantCommandHandler> logger, IMapper mapper)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
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

        mapper.Map(request.Dto, restaurant);

        await restaurantRepository.UpdateAsync(restaurant, cancellationToken);
        logger.LogInformation($"Restaurant with ID {request.Id} updated successfully");
    }
}
