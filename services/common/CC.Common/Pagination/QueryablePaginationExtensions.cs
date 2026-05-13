using System.Linq.Expressions;
using CC.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace CC.Common.Pagination;

public static class QueryablePaginationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PageInfo pageInfo,
        CancellationToken ct = default)
    {
        var totalRows = await query.CountAsync(ct);
        var items = await query
            .Skip(pageInfo.Skip)
            .Take(pageInfo.Size)
            .ToListAsync(ct);

        return PagedResult.From(items, pageInfo, totalRows);
    }

    public static async Task<PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
        this IQueryable<TSource> query,
        PageInfo pageInfo,
        Expression<Func<TSource, TResult>> selector,
        CancellationToken ct = default)
    {
        var totalRows = await query.CountAsync(ct);
        var items = await query
            .Skip(pageInfo.Skip)
            .Take(pageInfo.Size)
            .Select(selector)
            .ToListAsync(ct);

        return PagedResult.From(items, pageInfo, totalRows);
    }

    public static IQueryable<T> ApplyPaging<T>(
        this IQueryable<T> query,
        PageInfo pageInfo) =>
        query
            .Skip(pageInfo.Skip)
            .Take(pageInfo.Size);
}