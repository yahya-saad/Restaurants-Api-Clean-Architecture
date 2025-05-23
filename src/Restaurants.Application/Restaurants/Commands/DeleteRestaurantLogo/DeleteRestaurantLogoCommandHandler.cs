
using Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurantLogo;
internal class DeleteRestaurantLogoCommandHandler(
    ILogger<UploadRestaurantLogoCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IBlobStorageService blobStorageService) : IRequestHandler<DeleteRestaurantLogoCommand>
{
    public async Task Handle(DeleteRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleting restaurant logo for ID {request.RestaurantId}");
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId, cancellationToken);
        if (restaurant == null)
        {
            logger.LogWarning($"Restaurant with ID {request.RestaurantId} not found");
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourseOperation.Update))
            throw new ForbiddenException();

        if (restaurant.LogoUrl is not null)
        {
            await blobStorageService.DeleteAsync(restaurant.LogoUrl);
            restaurant.LogoUrl = null;
            await restaurantsRepository.UpdateAsync(restaurant);
        }


    }
}
