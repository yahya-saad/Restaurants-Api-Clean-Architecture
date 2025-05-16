namespace Restaurants.Application.Dishes.Commands.UpdateDish;
public class UpdateDishCommandHandler : IRequestHandler<UpdateDishCommand>
{
    private readonly IDishesRepository _dishRepository;
    private readonly IRestaurantsRepository _restaurantRepository;
    private readonly ILogger<UpdateDishCommandHandler> _logger;
    private readonly IMapper _mapper;
    public UpdateDishCommandHandler(
    IDishesRepository dishRepository,
        IRestaurantsRepository restaurantRepository,
        ILogger<UpdateDishCommandHandler> logger,
        IMapper mapper)
    {
        _dishRepository = dishRepository;
        _restaurantRepository = restaurantRepository;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
        if (restaurant is null)
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());

        var dish = await _dishRepository.GetByIdAsync(request.RestaurantId, request.DishId, cancellationToken);
        if (dish is null)
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());

        _mapper.Map(request.DishDto, dish);

        await _dishRepository.UpdateAsync(dish, cancellationToken);
        _logger.LogInformation("Dish with ID {DishId} updated successfully", request.DishId);
    }
}
