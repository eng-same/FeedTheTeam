
using Feed.Application.Interfaces;
using Feed.Application.Requests.Vote;
using Feed.Domain.Models;
using Mediator;

namespace Feed.Application.Commands.Vote;

public class AddVoteCommand: ICommand<int>
{
    public string CurrentUserId { get; set; } = string.Empty;
    public VoteRequest Dto { get; set; } 
}

public class AddVoteCommandHandler : ICommandHandler< AddVoteCommand , int>
{
    private readonly IVoteService _voteService;

    public AddVoteCommandHandler(IVoteService voteService)
    {
        _voteService = voteService;
    }

    public async ValueTask<int> Handle(AddVoteCommand command, CancellationToken ct)
    {
        var vote = await _voteService.AddVoteAsync(command.Dto.PoolId, command.Dto.OptionId, command.CurrentUserId);
        return vote.Id;
    }
}