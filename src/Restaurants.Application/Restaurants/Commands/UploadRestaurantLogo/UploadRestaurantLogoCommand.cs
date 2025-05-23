namespace Restaurants.Application.Restaurants.Commands.UploadRestaurantLogo;
public record UploadRestaurantLogoCommand(int RestaurantId, string FileName, Stream File) : IRequest;
