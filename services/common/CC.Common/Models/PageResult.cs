namespace CC.Common.Models;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    PageInfo Page,
    int TotalRows, int TotalPages)
{
    public bool HasPrevious => Page.Number > 1;
    public bool HasNext => Page.Number < TotalPages;
}