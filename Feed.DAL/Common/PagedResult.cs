
namespace Feed.Domain.Common;

public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public long TotalCount { get; init; }
    public int Page { get; init; }
    public int PageSize { get; init; }

    public PagedResult() { }

    public PagedResult(IEnumerable<T> items, long totalCount, int page, int pageSize)
    {
        Items = items?.ToList().AsReadOnly() ?? new List<T>().AsReadOnly();
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
    }
}
