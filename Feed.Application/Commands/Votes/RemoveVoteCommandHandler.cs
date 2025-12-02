
using Feed.Application.Interfaces;
using Mediator;

namespace Feed.Application.Commands.Vote;

public class RemoveVoteCommand : ICommand<bool>
{
    public int VoteId { get; set; }
    public string CurrentUserId { get; set; } = string.Empty;
}

public class RemoveVoteCommandHandler : ICommandHandler<RemoveVoteCommand, bool>
{
    private readonly IVoteService _voteService;

    public RemoveVoteCommandHandler(IVoteService voteService)
    {
        _voteService = voteService;
    }

    public async ValueTask<bool> Handle(RemoveVoteCommand command, CancellationToken ct)
    {
        await _voteService.RemoveVoteAsync(command.VoteId, command.CurrentUserId);
        return true;
    }
}