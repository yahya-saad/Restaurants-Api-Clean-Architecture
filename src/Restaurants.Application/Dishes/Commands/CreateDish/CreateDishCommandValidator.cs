namespace Restaurants.Application.Dishes.Commands.CreateDish;
public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(x => x.RestaurantId)
            .GreaterThan(0)
            .WithMessage("Restaurant ID must be greater than 0.");


        RuleFor(x => x.DishDto)
            .NotNull()
            .WithMessage("Dish DTO is required.");

        RuleFor(x => x.DishDto.Name)
            .NotEmpty()
            .WithMessage("Dish name is required.");

        RuleFor(x => x.DishDto.Description)
            .NotEmpty()
            .WithMessage("Dish description is required.");

        RuleFor(x => x.DishDto.Price)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Dish 'price' must be non-negative number.");


        RuleFor(x => x.DishDto.KiloCalories)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Dish 'KiloCalories' must be non-negative number.");
    }
}
