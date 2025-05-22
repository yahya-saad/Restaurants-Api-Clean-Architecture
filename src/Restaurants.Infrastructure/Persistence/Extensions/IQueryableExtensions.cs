using Restaurants.Domain.Constants;
using System.Linq.Dynamic.Core;


namespace Restaurants.Infrastructure.Persistence.Extensions;
internal static class IQueryableExtensions
{
    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int PageNumber, int PageSize)
    {
        return query
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize);
    }

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortBy, SortDirection? sortDirection)
    {
        if (string.IsNullOrEmpty(sortBy))
            return query;

        var direction = sortDirection == SortDirection.Descending ? "descending" : "ascending";
        var sortingString = $"{sortBy} {direction}";


        try
        {
            return query.OrderBy(sortingString);
        }
        catch
        {
            return query;
        }

    }
}