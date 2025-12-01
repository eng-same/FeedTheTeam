
using Feed.Application.DTOs.Vote;
using Feed.Application.Interfaces;
using Feed.Domain.Models;
using Mediator;

namespace Feed.Application.Queries.Votes;

public class GetVotesByPoolQuery : IQuery<VoteSummaryDto?>
{
    public int PoolId { get; set; }
}

public class GetVotesByPoolQueryHandler : IQueryHandler<GetVotesByPoolQuery, VoteSummaryDto?>
{
    private readonly IVoteService _voteService;

    public GetVotesByPoolQueryHandler(IVoteService voteService)
    {
        _voteService = voteService;
    }

    public async ValueTask<VoteSummaryDto?> Handle(GetVotesByPoolQuery query, CancellationToken ct)
    {
        return await _voteService.GetVotesByPoolAsync(query.PoolId);
    }
}