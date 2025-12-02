using Feed.Application.Interfaces;
using Feed.Application.Requests.Pool;
using Mediator;

namespace Feed.Application.Commands.Pool;

public class ClosePoolCommand : ICommand<bool>
{
    public int PoolId { get; set; }
    public string CurrentUserId { get; set; }
}

public class ClosePoolCommandHandler : ICommandHandler<ClosePoolCommand, bool>
{
    private readonly IPoolService _poolService;

    public ClosePoolCommandHandler(IPoolService poolService)
    {
        _poolService = poolService;
    }

    public async ValueTask<bool> Handle(ClosePoolCommand command, CancellationToken ct)
    {
        await _poolService.ClosePoolAsync(command.PoolId, command.CurrentUserId);
        return true;
    }
}


