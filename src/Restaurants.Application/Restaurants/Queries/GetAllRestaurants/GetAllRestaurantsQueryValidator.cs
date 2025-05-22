namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
public class GetAllRestaurantsQueryValidator : AbstractValidator<GetAllRestaurantsQuery>
{
    private readonly List<int> allowedPageSizes = [5, 10, 15, 30];
    private readonly List<string> allowedSortBy = [nameof(Restaurant.Name), nameof(Restaurant.Category), nameof(Restaurant.Description)];
    public GetAllRestaurantsQueryValidator()
    {
        RuleFor(x => x.pageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");
        RuleFor(x => x.pageSize)
                .Must(value => allowedPageSizes.Contains(value))
                .WithMessage($"Page size must be in [{string.Join(",", allowedPageSizes)}] .");

        RuleFor(x => x.SortBy)
            .Must(value => allowedSortBy.Contains(value))
            .When(x => x.SortBy is not null)
            .WithMessage($"SortBy is optional, or must be in [{string.Join(",", allowedSortBy)}].");
    }
}
