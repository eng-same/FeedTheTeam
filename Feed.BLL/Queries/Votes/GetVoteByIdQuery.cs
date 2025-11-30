
using Feed.Application.Interfaces;
using Feed.Domain.Models;
using Mediator;

namespace Feed.Application.Queries.Votes;

public class GetVoteByIdQuery : IQuery<Vote?>
{
    public int VoteId { get; set; }
}

public class GetVoteByIdQueryHandler : IQueryHandler<GetVoteByIdQuery, Vote?>
{
    private readonly IVoteService _voteService;

    public GetVoteByIdQueryHandler(IVoteService voteService)
    {
        _voteService = voteService;
    }

    public async ValueTask<Vote?> Handle(GetVoteByIdQuery query, CancellationToken ct)
    {
        return await _voteService.GetVoteByIdAsync(query.VoteId);
    }
}