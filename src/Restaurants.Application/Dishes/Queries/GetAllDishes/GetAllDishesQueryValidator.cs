namespace Restaurants.Application.Dishes.Queries.GetAllDishes;
public class GetAllDishesQueryValidator : AbstractValidator<GetAllDishesQuery>
{
    public GetAllDishesQueryValidator()
    {
        RuleFor(x => x.RestaurantId)
            .GreaterThan(0)
            .WithMessage("Restaurant ID must be greater than 0.");
    }
}
