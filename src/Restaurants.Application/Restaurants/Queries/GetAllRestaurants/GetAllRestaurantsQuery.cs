using Restaurants.Application.Common.Pagination;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
public record GetAllRestaurantsQuery(string? search, int pageNumber, int pageSize, string? SortBy, SortDirection? SortDirection) : IRequest<PaginatedResult<RestaurantDto>>;
