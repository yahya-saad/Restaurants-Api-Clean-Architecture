using FluentValidation;
using Restaurants.Application.Restaurants.DTOs;

namespace Restaurants.Application.Restaurants.Validators;
public class CreateRestaurantDtoValidator : AbstractValidator<CreateResaturantDto>
{
    private readonly string[] _validCategories = new[] { "Fast Food", "Egyptian", "Italian", "Chinese", "Japanese" };
    public CreateRestaurantDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(3)
            .WithMessage("Name must be at least 3 characters long.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Category)
           .Must(x => _validCategories.Contains(x))
           .WithMessage($"Category must be one of the following: {string.Join(", ", _validCategories)}.");

        RuleFor(x => x.ContractEmail)
            .EmailAddress()
            .WithMessage("Please provide a valid email address.");

        RuleFor(x => x.ContractNumber)
            .Matches(@"^(?:\+20|0)?1[0125][0-9]{8}$") // egyptian numbers
            .WithMessage("Please provide a valid phone number.");

        RuleFor(x => x.Address.PostalCode)
            .Matches(@"^\d{5}$")
            .WithMessage("Please provide a valid postal code.");
    }
}
