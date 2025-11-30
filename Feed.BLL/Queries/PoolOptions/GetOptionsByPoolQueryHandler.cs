
using Feed.Application.Interfaces;
using Feed.Application.DTOs.PoolOption;
using Mediator;

namespace Feed.Application.Queries.PoolOptions;

public class GetOptionsByPoolQuery : IQuery<IEnumerable<PoolOptionDto>>
{
    public int PoolId { get; set; }
}

public class GetOptionsByPoolQueryHandler : IQueryHandler<GetOptionsByPoolQuery, IEnumerable<PoolOptionDto>>
{
    private readonly IPoolOptionService _optionService;

    public GetOptionsByPoolQueryHandler(IPoolOptionService optionService)
    {
        _optionService = optionService;
    }

    public async ValueTask<IEnumerable<PoolOptionDto>> Handle(GetOptionsByPoolQuery query, CancellationToken ct)
    {
        return await _optionService.GetOptionsByPoolAsync(query.PoolId);
    }
}