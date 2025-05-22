using Restaurants.Application.Common.Pagination;
using System.Linq.Expressions;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, PaginatedResult<RestaurantDto>>
{
    private readonly IRestaurantsRepository restaurantRepository;
    private readonly ILogger<GetAllRestaurantsQueryHandler> logger;
    private readonly IMapper mapper;
    public GetAllRestaurantsQueryHandler(
        IRestaurantsRepository restaurantRepository,
        ILogger<GetAllRestaurantsQueryHandler> logger,
        IMapper mapper)
    {
        this.restaurantRepository = restaurantRepository;
        this.logger = logger;
        this.mapper = mapper;
    }
    public async Task<PaginatedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching all restaurants");
        var search = request.search?.Trim();
        Expression<Func<Restaurant, bool>>? filter = r => search == null || r.Name.Contains(search) || r.Description.Contains(search);

        var (restaurants, count) = await restaurantRepository.GetAllAsync(
            request.pageSize, request.pageNumber,
            filter: filter,
            SortBy: request.SortBy, sortDirection: request.SortDirection,
            includeProperties: "Dishes",
            cancellationToken: cancellationToken);

        var dtoList = mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

        var paginatedResult = new PaginatedResult<RestaurantDto>
        {
            Data = dtoList,
            Pagination = new PaginationMetadata
            {
                PageNumber = request.pageNumber,
                PageSize = request.pageSize,
                TotalCount = count,
                TotalPages = (int)Math.Ceiling((double)count / request.pageSize)
            }
        };

        return paginatedResult;
    }
}

