

using Feed.Application.Interfaces;
using Feed.Application.Requests.Pool;
using Mediator;

namespace Feed.Application.Commands.Pool;

public class UpdatePoolCommand : ICommand<bool>
{ 
    public UpdatePoolRequest updatePoolRequest { get; set; } 
    public string CurrentUserId { get; set; }
}

public class UpdatePoolCommandHandler : ICommandHandler<UpdatePoolCommand, bool>
{
    private readonly IPoolService _poolService;

    public UpdatePoolCommandHandler(IPoolService poolService)
    {
        _poolService = poolService;
    }

    public async ValueTask<bool> Handle(UpdatePoolCommand command, CancellationToken ct)
    {
        await _poolService.UpdatePoolAsync(command.updatePoolRequest, command.CurrentUserId);
        return true;
    }
}

