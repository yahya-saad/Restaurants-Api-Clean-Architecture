
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;
internal class UploadRestaurantLogoCommandHandler(
    ILogger<UploadRestaurantLogoCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService,
    IBlobStorageService blobStorageService) : IRequestHandler<UploadRestaurantLogoCommand>
{
    public async Task Handle(UploadRestaurantLogoCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Updateing restaurant logo for ID {request.RestaurantId}");
        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId, cancellationToken);
        if (restaurant == null)
        {
            logger.LogWarning($"Restaurant with ID {request.RestaurantId} not found");
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourseOperation.Update))
            throw new ForbiddenException();

        var logoUrl = await blobStorageService.UploadAsync(request.FileName, request.File);
        restaurant.LogoUrl = logoUrl;

        await restaurantsRepository.UpdateAsync(restaurant);

    }
}
