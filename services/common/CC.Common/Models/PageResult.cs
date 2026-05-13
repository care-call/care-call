namespace CC.Common.Models;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    PageInfo Page,
    int TotalRows, int TotalPages)
{
    public bool HasPrevious => Page.Number > 1;
    public bool HasNext => Page.Number < TotalPages;

    public static PagedResult<T> From(
        IReadOnlyList<T> items,
        PageInfo page,
        int totalRows) =>
        new(items, page, totalRows, CalculateTotalPages(totalRows, page.Size));

    private static int CalculateTotalPages(int totalRows, int pageSize) =>
        totalRows == 0 ? 0 : (int)Math.Ceiling(totalRows / (double)pageSize);
}