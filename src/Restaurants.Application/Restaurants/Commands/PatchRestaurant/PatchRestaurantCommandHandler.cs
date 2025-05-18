using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Commands.PatchRestaurant;
public class PatchRestaurantCommandHandler : IRequestHandler<PatchRestaurantCommand>
{
    private readonly IRestaurantsRepository restaurantRepository;
    private readonly ILogger<PatchRestaurantCommandHandler> logger;
    private readonly IMapper mapper;
    private readonly IRestaurantAuthorizationService restaurantAuthorizationService;
    public PatchRestaurantCommandHandler(IRestaurantsRepository restaurantRepository, ILogger<PatchRestaurantCommandHandler> logger, IMapper mapper, IRestaurantAuthorizationService restaurantAuthorizationService)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
        this.restaurantAuthorizationService = restaurantAuthorizationService;
    }

    public async Task Handle(PatchRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Patching restaurant with id {request.Id}");

        var restaurant = await restaurantRepository.GetByIdAsync(request.Id, cancellationToken);
        if (restaurant == null)
        {
            logger.LogWarning($"Restaurant with id {request.Id} not found");
            throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourseOperation.Delete))
            throw new ForbiddenException();

        var dto = mapper.Map<PatchRestaurantDto>(restaurant);

        try
        {
            request.PatchDocument.ApplyTo(dto);
        }
        catch (JsonPatchException ex)
        {
            throw new ValidationException(new[] { new ValidationFailure("PatchDocument", ex.Message) });
        }


        mapper.Map(dto, restaurant);

        await restaurantRepository.UpdateAsync(restaurant, cancellationToken);
        logger.LogInformation($"Restaurant with id {request.Id} patched");
    }
}


