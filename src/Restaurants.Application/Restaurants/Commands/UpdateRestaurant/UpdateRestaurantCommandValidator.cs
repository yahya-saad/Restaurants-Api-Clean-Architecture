namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
public class UpdateRestaurantCommandValidator : AbstractValidator<UpdateRestaurantCommand>
{
    private readonly string[] _validCategories = new[] { "Fast Food", "Egyptian", "Italian", "Chinese", "Japanese" };
    public UpdateRestaurantCommandValidator()
    {
        RuleFor(x => x.Dto.Name)
                   .NotEmpty()
                   .WithMessage("Name is required.")
                   .MinimumLength(3)
                   .WithMessage("Name must be at least 3 characters long.")
                   .MaximumLength(100)
                   .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Dto.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Dto.Category)
           .Must(x => _validCategories.Contains(x))
           .WithMessage($"Category must be one of the following: {string.Join(", ", _validCategories)}.");

    }
}
