namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;
public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{
    private readonly string[] _validCategories = new[] { "Fast Food", "Egyptian", "Italian", "Chinese", "Japanese" };
    public CreateRestaurantCommandValidator()
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

        RuleFor(x => x.Dto.ContractEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address.");

        RuleFor(x => x.Dto.ContractNumber)
            .Matches(@"^(?:\+20|0)?1[0125][0-9]{8}$") // egyptian numbers
            .WithMessage("Please provide a valid phone number.");

        RuleFor(x => x.Dto.Address.PostalCode)
            .Matches(@"^\d{5}$")
            .WithMessage("Please provide a valid postal code.");
    }

}
