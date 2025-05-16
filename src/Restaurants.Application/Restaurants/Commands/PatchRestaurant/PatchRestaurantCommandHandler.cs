using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch.Exceptions;

namespace Restaurants.Application.Restaurants.Commands.PatchRestaurant;
public class PatchRestaurantCommandHandler : IRequestHandler<PatchRestaurantCommand>
{
    private readonly IRestaurantRepository restaurantRepository;
    private readonly ILogger<PatchRestaurantCommandHandler> logger;
    private readonly IMapper mapper;
    public PatchRestaurantCommandHandler(IRestaurantRepository restaurantRepository, ILogger<PatchRestaurantCommandHandler> logger, IMapper mapper)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
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


