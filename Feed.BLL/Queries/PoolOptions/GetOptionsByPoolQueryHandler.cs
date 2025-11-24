
using Mediator;

namespace Feed.Application.Queries.PoolOptions;

public class GetOptionsByPoolQueryHandler : IRequestHandler<GetPoolOptionsQuery, IEnumerable<PoolDto>>
{
    public ValueTask<IEnumerable<PoolDto>> Handle(GetPoolOptionsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

public class GetPoolOptionsQuery : IRequest<IEnumerable<PoolDto>>
{
    public int PoolId { get; set; }
}
