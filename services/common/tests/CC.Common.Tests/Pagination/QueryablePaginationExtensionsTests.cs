using CC.Common.Models;
using CC.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CC.Common.Tests.Pagination;

public sealed class QueryablePaginationExtensionsTests
{
    [Fact]
    public async Task Second_page_contains_next_ten_items()
    {
        await using var dbContext = await CreateDbContextWithItems(25);

        var result = await dbContext.Items
            .OrderBy(x => x.Value)
            .ToPagedResultAsync(new PageInfo(2, 10), CancellationToken.None);

        Assert.Equal([11, 12, 13, 14, 15, 16, 17, 18, 19, 20], result.Items.Select(x => x.Value));
        Assert.Equal(25, result.TotalRows);
        Assert.Equal(3, result.TotalPages);
        Assert.True(result.HasPrevious);
        Assert.True(result.HasNext);
    }

    [Fact]
    public async Task Empty_query_returns_empty_page()
    {
        await using var dbContext = CreateDbContext();

        var result = await dbContext.Items
            .OrderBy(x => x.Value)
            .ToPagedResultAsync(new PageInfo(1, 10), CancellationToken.None);

        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalRows);
        Assert.Equal(0, result.TotalPages);
        Assert.False(result.HasPrevious);
        Assert.False(result.HasNext);
    }

    [Fact]
    public async Task Projection_returns_dto_page()
    {
        await using var dbContext = await CreateDbContextWithItems(5);

        var result = await dbContext.Items
            .OrderBy(x => x.Value)
            .ToPagedResultAsync(
                new PageInfo(2, 2),
                x => new TestItemDto(x.Value),
                CancellationToken.None);

        Assert.Equal([new TestItemDto(3), new TestItemDto(4)], result.Items);
        Assert.Equal(5, result.TotalRows);
        Assert.Equal(3, result.TotalPages);
    }

    private static async Task<TestDbContext> CreateDbContextWithItems(int count)
    {
        var dbContext = CreateDbContext();
        dbContext.Items.AddRange(Enumerable.Range(1, count).Select(x => new TestItem { Value = x }));
        await dbContext.SaveChangesAsync();

        return dbContext;
    }

    private static TestDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TestDbContext(options);
    }

    private sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<TestItem> Items => Set<TestItem>();
    }

    private sealed class TestItem
    {
        public int Id { get; set; }
        public int Value { get; set; }
    }

    private sealed record TestItemDto(int Value);
}