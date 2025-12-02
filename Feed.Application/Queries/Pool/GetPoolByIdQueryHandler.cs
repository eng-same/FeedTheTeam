using Feed.Application.Interfaces;
using FluentValidation;
using Mediator;

namespace Feed.Application.Queries.Pool;

public class GetPoolByIdQuery : IQuery<PoolDto?>
{
    public int PoolId { get; set; }
}

public class GetPoolByIdQueryHandler
    : IQueryHandler<GetPoolByIdQuery, PoolDto?>
{
    private readonly IPoolService _poolService;

    public GetPoolByIdQueryHandler(IPoolService poolService)
    {
        _poolService = poolService;
    }

    public async ValueTask<PoolDto?> Handle(GetPoolByIdQuery query, CancellationToken ct)
    {
        return await _poolService.GetPoolByIdAsync(query.PoolId);
    }
}

//public class GetPoolByIdValidator: AbstractValidator<GetPoolByIdQuery>
//{
//    // valida
//}