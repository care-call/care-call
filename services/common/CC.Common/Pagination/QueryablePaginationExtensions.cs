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
            .ApplyPaging(pageInfo)
            .ToListAsync(ct);

        return PagedResult<T>.From(items, pageInfo, totalRows);
    }

    public static async Task<PagedResult<TResult>> ToPagedResultAsync<TSource, TResult>(
        this IQueryable<TSource> query,
        PageInfo pageInfo,
        Expression<Func<TSource, TResult>> selector,
        CancellationToken ct = default)
    {
        var totalRows = await query.CountAsync(ct);
        var items = await query
            .ApplyPaging(pageInfo)
            .Select(selector)
            .ToListAsync(ct);

        return PagedResult<TResult>.From(items, pageInfo, totalRows);
    }

    public static IQueryable<T> ApplyPaging<T>(
        this IQueryable<T> query,
        PageInfo pageInfo) => query.Skip(GetSkip(pageInfo)).Take(pageInfo.Size);

    private static int GetSkip(PageInfo pageInfo) =>
        checked((pageInfo.Number - 1) * pageInfo.Size);
}