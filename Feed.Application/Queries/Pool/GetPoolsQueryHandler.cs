using Feed.Application.Interfaces;
using Feed.Domain.Common;
using Mediator;

namespace Feed.Application.Queries.Pool;

public class GetPoolsQuery : IQuery<PagedResult<PoolDto>>
{
    public PoolFilter Filter { get; set; }
}

public class GetPoolsQueryHandler
    : IQueryHandler<GetPoolsQuery, PagedResult<PoolDto>>
{
    private readonly IPoolService _poolService;

    public GetPoolsQueryHandler(IPoolService poolService)
    {
        _poolService = poolService;
    }

    public async ValueTask<PagedResult<PoolDto>> Handle(GetPoolsQuery query, CancellationToken ct)
    {
        return await _poolService.GetPoolsAsync(query.Filter);
    }
}