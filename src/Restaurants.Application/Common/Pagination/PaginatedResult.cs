namespace Restaurants.Application.Common.Pagination;
public class PaginatedResult<T>
{
    public IEnumerable<T> Data { get; set; } = new List<T>();
    public PaginationMetadata Pagination { get; set; } = new();
}
