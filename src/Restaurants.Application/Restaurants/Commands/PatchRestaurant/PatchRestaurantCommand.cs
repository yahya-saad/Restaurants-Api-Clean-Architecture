using Microsoft.AspNetCore.JsonPatch;
namespace Restaurants.Application.Restaurants.Commands.PatchRestaurant;
public record PatchRestaurantCommand(int Id, JsonPatchDocument<PatchRestaurantDto> PatchDocument) : IRequest;