using System.Linq.Expressions;
using CC.Common.Models;
using CC.HandbookService.Application.Enums;
using Microsoft.EntityFrameworkCore;

namespace CC.HandbookService.Infrastructure.Persistence.Extensions;

public static class IQueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, PageInfo pageInfo)
    {
        var totalRows = await query.CountAsync();
        
        var items = await query
            .Skip((pageInfo.Number - 1) * pageInfo.Size)
            .Take(pageInfo.Size)
            .ToListAsync();

        return new PagedResult<T>(items, pageInfo, totalRows);
    }
    
    public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, string searchBy, string searchValue)
    {
        if (string.IsNullOrWhiteSpace(searchBy) || searchValue == null)
            return query;

        var propertyInfo = typeof(T).GetProperty(searchBy);
        if (propertyInfo is null)
            throw new ArgumentException($"Property {searchBy} not found");
        
        var param = Expression.Parameter(typeof(T));
        var property = Expression.Property(param, propertyInfo);
        
        object? convertedValue;
        var underlyingType  = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
        
        if (underlyingType == typeof(Guid))
            convertedValue = Guid.Parse(searchValue);
        else if (underlyingType.IsEnum)
            convertedValue = Enum.Parse(underlyingType, searchValue);
        else
            convertedValue = Convert.ChangeType(searchValue, underlyingType);
        
        var equal = Expression.Equal(property, Expression.Constant(convertedValue, propertyInfo.PropertyType));
        
        var lambda = Expression.Lambda<Func<T, bool>>(equal, param);

        return query.Where(lambda);
    }
    
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, string sortBy, SortOrder sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;
        
        var propertyInfo = typeof(T).GetProperty(sortBy);
        if (propertyInfo is null)
            throw new ArgumentException($"Property {sortBy} not found");
        
        var param = Expression.Parameter(typeof(T));
        var property = Expression.Property(param, propertyInfo);
        
        var lambda = Expression.Lambda<Func<T, object>>(
            Expression.Convert(property, typeof(object)), 
            param);
        
        if (sortOrder == SortOrder.Asc)
            return query.OrderBy(lambda);

        return query.OrderByDescending(lambda);
    }
}