using System.Linq.Expressions;
using CC.Common.Models;
using CC.HandbookService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace CC.HandbookService.Infrastructure.Persistence.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, PageInfo pageInfo)
    {
        var totalRows = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalRows / (double)pageInfo.Size);
        var items = await query
            .Skip((pageInfo.Number - 1) * pageInfo.Size)
            .Take(pageInfo.Size)
            .ToListAsync();

        return new PagedResult<T>(items, pageInfo, totalRows, totalPages);
    }
    
    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, string searchBy, string searchValue)
    {
        var propertyInfo = typeof(T).GetProperty(searchBy);
        if (propertyInfo is null)
            throw new ArgumentException($"Property {searchBy} not found");
        
        var param = Expression.Parameter(typeof(T));
        var property = Expression.Property(param, propertyInfo);
        
        var underlyingType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
        if (!TryConvert(searchValue, underlyingType, out var convertedValue))
            throw new ArgumentException($"Invalid value '{searchValue}' for property '{searchBy}' of type '{underlyingType.Name}'");
        
        var equal = Expression.Equal(property, Expression.Constant(convertedValue, propertyInfo.PropertyType));
        
        var lambda = Expression.Lambda<Func<T, bool>>(equal, param);

        return query.Where(lambda);
    }
    
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string sortBy, SortOrder sortOrder)
    {
        var propertyInfo = typeof(T).GetProperty(sortBy);
        if (propertyInfo is null)
            throw new ArgumentException($"Property {sortBy} not found");
        
        var param = Expression.Parameter(typeof(T));
        var property = Expression.Property(param, propertyInfo);
        
        var lambda = Expression.Lambda<Func<T, object>>(
            Expression.Convert(property, typeof(object)), 
            param);
        
        if (sortOrder == SortOrder.Ascending)
            return query.OrderBy(lambda);

        return query.OrderByDescending(lambda);
    }
    
    private static bool TryConvert(string value, Type targetType, out object? result)
    {
        result = null;
        
        try
        {
            if (targetType == typeof(Guid))
            {
                if (!Guid.TryParse(value, out var guid)) return false;
                result = guid;
                return true;
            }

            if (targetType.IsEnum)
            {
                if (!Enum.TryParse(targetType, value, ignoreCase: true, out var parsed)) return false;
                result = parsed;
                return true;
            }

            result = Convert.ChangeType(value, targetType);
            return true;
        }
        catch
        {
            return false;
        }
    }
}