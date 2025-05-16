namespace Restaurants.Application.Dishes.Queries.GetDish;
public class GetDishQueryValidator : AbstractValidator<GetDishQuery>
{
    public GetDishQueryValidator()
    {
        RuleFor(x => x.RestaurantId)
            .GreaterThan(0)
            .WithMessage("Restaurant ID must be greater than 0.");

        RuleFor(x => x.DishId)
            .GreaterThan(0)
            .WithMessage("Dish ID must be greater than 0.");
    }
}

