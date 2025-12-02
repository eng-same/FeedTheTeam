
using Feed.Application.Interfaces;
using Feed.Application.Requests.Vote;
using Feed.Domain.Models;
using FluentValidation;
using Mediator;

namespace Feed.Application.Commands.Vote;

public class AddVoteCommand: ICommand<int>
{
    public string CurrentUserId { get; set; } = string.Empty;
    public VoteRequest Dto { get; set; } 
}

public class VoteRequestValidator : AbstractValidator<VoteRequest>
{
    public VoteRequestValidator()
    {
        RuleFor(x => x.PoolId)
            .GreaterThan(0).WithMessage("PollId must be a positive integer.");
        RuleFor(x => x.OptionId)
            .GreaterThan(0).WithMessage("OptionId must be a positive integer.");
    }
}

public class AddVoteCommandHandler : ICommandHandler< AddVoteCommand , int>
{
    private readonly IVoteService _voteService;
    private readonly IValidator<VoteRequest> _validator;

    public AddVoteCommandHandler(IVoteService voteService, IValidator<VoteRequest> validator)
    {
        _voteService = voteService;
        _validator = validator;

    }

    public async ValueTask<int> Handle(AddVoteCommand command, CancellationToken ct)
    {
        await _validator.ValidateAndThrowAsync(command.Dto, ct);
        var vote = await _voteService.AddVoteAsync(command.Dto.PoolId, command.Dto.OptionId, command.CurrentUserId);
        return vote.Id;
    }
}