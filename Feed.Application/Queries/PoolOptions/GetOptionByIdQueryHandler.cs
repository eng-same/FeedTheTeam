
using Feed.Application.Interfaces;
using Feed.Application.DTOs.PoolOption;
using Mediator;

namespace Feed.Application.Queries.PoolOptions;

public class GetOptionByIdQuery : IQuery<PoolOptionDto?>
{
    public int OptionId { get; set; }
}

public class GetOptionByIdQueryHandler : IQueryHandler<GetOptionByIdQuery, PoolOptionDto?>
{
    private readonly IPoolOptionService _optionService;

    public GetOptionByIdQueryHandler(IPoolOptionService optionService)
    {
        _optionService = optionService;
    }

    public async ValueTask<PoolOptionDto?> Handle(GetOptionByIdQuery query, CancellationToken ct)
    {
        return await _optionService.GetOptionByIdAsync(query.OptionId);
    }
}