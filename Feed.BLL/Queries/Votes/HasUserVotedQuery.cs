
using Feed.Application.Interfaces;
using Mediator;

namespace Feed.Application.Queries.Votes;

public class HasUserVotedQuery : IQuery<bool>
{
    public int PoolId { get; set; }
    public string UserId { get; set; } = string.Empty;
}

public class HasUserVotedQueryHandler : IQueryHandler<HasUserVotedQuery, bool>
{
    private readonly IVoteService _voteService;

    public HasUserVotedQueryHandler(IVoteService voteService)
    {
        _voteService = voteService;
    }

    public async ValueTask<bool> Handle(HasUserVotedQuery query, CancellationToken ct)
    {
        return await _voteService.HasUserVotedAsync(query.PoolId, query.UserId);
    }
}