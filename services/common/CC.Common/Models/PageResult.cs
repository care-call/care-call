namespace CC.Common.Models;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    PageInfo Page,
    int TotalRows);